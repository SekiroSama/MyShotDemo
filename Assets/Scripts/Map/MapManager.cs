using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private int[][] map = {
        new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
    };

    public Vector3 startPos;
    public Transform mapParent;
    //public Material material_Wall;
    public GameObject wallPrefab;

    private void Start()
    {
        InitMap();
    }

    public void InitMap()
    {
        // 1. 获取地图的宽和高 (假设地图是矩形)
        int rows = map.Length;    // 行数 (高)
        int cols = map[0].Length; // 列数 (宽)

        // --- 第一步：生成视觉层 (Visuals) ---
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (map[i][j] == 1)
                {
                    GameObject cube = Instantiate(wallPrefab);

                    // 设置位置
                    cube.transform.position = new Vector3(j, -i, 0) + startPos;
                    cube.transform.SetParent(mapParent);

                    // 设置图层和Tag
                    cube.layer = LayerMask.NameToLayer("Wall");
                    cube.tag = "Wall";
                }
            }
        }

        // --- 第二步：生成物理层 (Physics - 4个大碰撞体) ---
        CreateBigColliders(rows, cols);
    }

    // 专门用来生成四周大墙壁的函数
    private void CreateBigColliders(int rows, int cols)
    {
        // 计算中心点坐标偏移量
        // 因为你的地图是从 (0,0) 到 (cols-1, -(rows-1))
        float centerX = (cols - 1) / 2.0f;
        float centerY = -(rows - 1) / 2.0f;

        // 1. 上墙 (Top)
        // 位置：X在中心，Y在 0，Z在 0
        // 尺寸：宽=cols，高=1，厚=1
        CreateOneCollider("Wall_Top",
            new Vector3(centerX, 0, 0) + startPos,
            new Vector3(cols, 1, 1));

        // 2. 下墙 (Bottom)
        // 位置：X在中心，Y在 最下面(-rows + 1)，Z在 0
        CreateOneCollider("Wall_Bottom",
            new Vector3(centerX, -(rows - 1), 0) + startPos,
            new Vector3(cols, 1, 1));

        // 3. 左墙 (Left)
        // 位置：X在 0，Y在中心，Z在 0
        // 尺寸：宽=1，高=rows，厚=1
        CreateOneCollider("Wall_Left",
            new Vector3(0, centerY, 0) + startPos,
            new Vector3(1, rows, 1));

        // 4. 右墙 (Right)
        // 位置：X在 最右边(cols - 1)，Y在中心，Z在 0
        CreateOneCollider("Wall_Right",
            new Vector3(cols - 1, centerY, 0) + startPos,
            new Vector3(1, rows, 1));
    }

    // 生成单个大碰撞体的通用方法
    private void CreateOneCollider(string name, Vector3 pos, Vector3 size)
    {
        GameObject wall = new GameObject(name);
        wall.transform.position = pos;
        wall.transform.SetParent(mapParent);

        // 添加 BoxCollider
        BoxCollider box = wall.AddComponent<BoxCollider>();
        box.size = size;

        // 设置物理属性
        wall.layer = LayerMask.NameToLayer("Wall");
        wall.tag = "Wall";

        // 设置为静态（优化性能）
        wall.isStatic = true;
    }
}
