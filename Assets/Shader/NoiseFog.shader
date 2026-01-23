Shader "Unlit/NoiseFog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor("FogColor", Color) = (1,1,1,1)
        _FogDensity("FogDensity", Float) = 1
        _FogStart("FogStart", Float) = 0
        _FogEnd("FogEnd", Float) = 10
        //决定不均匀效果的噪声纹理
        _Noise("Noise", 2D) = ""{}
        //噪声系数 控制 偏移范围的
        _NoiseAmount("NoiseAmount", Float) = 1
        //用于控制动态移动速度的
        _FogXSpeed("FogXSpeed", Float) = 0.1
        _FogYSpeed("FogYSpeed", Float) = 0.1
    }
    SubShader
    {
        ZTest Always
        Cull Off
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct v2f
            {
                float2 uv : TEXCOORD0;
                //深度纹理UV
                float2 uv_depth : TEXCOORD1;
                //顶点射线 指向四个角的方向向量 （传递到片元时 会自动进行插值 运算）
                float4 ray:TEXCOORD2;
                float4 vertex : SV_POSITION;
            };
            sampler2D _MainTex;
            //纹素 用来判断翻转会使用
            half4 _MainTex_TexelSize;
            //深度纹理
            sampler2D _CameraDepthTexture;
            //雾相关的属性
            fixed4 _FogColor;
            fixed _FogDensity;
            float _FogStart;
            float _FogEnd;
            //矩阵相关 里面存储了 4条射线向量
            //0-左下 1-右下 2-右上 3-左上
            float4x4 _RayMatrix;
            sampler2D _Noise;
            float _NoiseAmount;
            float _FogXSpeed;
            float _FogYSpeed;
            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.uv_depth = v.texcoord;
                //顶点着色器函数 每一个顶点都会执行一次
                //对于屏幕后处理来说 就会执行4次 因为有4个顶点 （4个角）
                //通过uv坐标判断 当前的顶点位置
                int index = 0;
                if(v.texcoord.x < 0.5 && v.texcoord.y < 0.5)
                    index = 0;
                else if(v.texcoord.x > 0.5 && v.texcoord.y < 0.5)
                    index = 1;
                else if(v.texcoord.x > 0.5 && v.texcoord.y > 0.5)
                    index = 2;
                else
                    index = 3;
                //判断 是否需要进行纹理翻转 如果翻转了 深度的uv和对应顶点需要变化
                #if UNITY_UV_STARTS_AT_TOP
                if(_MainTex_TexelSize.y < 0)
                {
                    o.uv_depth.y = 1 - o.uv_depth.y;
                    index = 3 - index;
                }
                #endif
                //根据顶点的位置 决定使用那一个射线向量
                o.ray = _RayMatrix[index];
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                //观察空间下 离摄像机的实际距离（Z分量）
                float linearDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth));
                //计算世界空间下 像素的坐标
                float3 worldPos = _WorldSpaceCameraPos + linearDepth * i.ray;
               
                //噪声纹理采样以及偏移
                float2 speed = _Time.y * float2(_FogXSpeed, _FogYSpeed);
                //噪声纹理中采样 采样出来的结果是0~1 我们需要将其转为 -0.5~0.5 然后还可以通过自定义系数控制这个正负范围
                float noise = ( tex2D(_Noise, i.uv + speed).r - 0.5 ) *_NoiseAmount;
               
                //雾相关的计算
                //混合因子
                float f = (_FogEnd - worldPos.y)/(_FogEnd - _FogStart);
                //取0~1之间 超过会取极值
                //之所以乘以1 + noise，是为了在正常计算出来的混合因子中去进行 上下的扰动
                f = saturate(f * _FogDensity * (1 + noise));
                //利用插值 在两个颜色之间进行融合
                fixed3 color = lerp(tex2D(_MainTex, i.uv).rgb, _FogColor.rgb, f);
                return fixed4(color.rgb, 1);
            }
            ENDCG
        }
    }
    Fallback Off
}
