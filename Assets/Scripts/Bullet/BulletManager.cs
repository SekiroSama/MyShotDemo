using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        RayCheckHitEnemy();
    }

    private void Move()
    {
        this.transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyManager>().TakeDamage();
            DestroySelf();
        }
    }

    private void RayCheckHitEnemy()
    {
        //3.获取相交的多个物体
        //可以得到碰撞到的多个对象
        //如果没有 就是容量为0的数组
        //参数一：射线
        //参数二：距离
        //参数三：检测指定层级（不填检测所有层）
        //参数四：是否忽略触发器 UseGlobal-使用全局设置 Collide-检测触发器 Ignore-忽略触发器 不填使用UseGlobal
        Ray r3 = new Ray(Vector3.zero, Vector3.forward);
        RaycastHit[] hits = Physics.RaycastAll(r3, 1000, 1 << LayerMask.NameToLayer("Monster"), QueryTriggerInteraction.UseGlobal);
        for (int i = 0; i < hits.Length; i++)
        {
            print("碰到的所有物体 名字分别是" + hits[i].collider.gameObject.name);
        }
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
