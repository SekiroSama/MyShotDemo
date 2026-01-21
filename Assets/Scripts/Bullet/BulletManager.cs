using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed;


    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 2f);
        lastPos = this.transform.position;
    }

    void Update()
    {
        Move();

        RayCheckHitEnemy();

    }

    private void Move()
    {
        // print(this.transform.right);
        this.transform.Translate(speed * Time.deltaTime * this.transform.right, Space.World);//默认是局部坐标
        //this.transform.position = this.transform.position + speed * Time.deltaTime * this.transform.right;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            HitEnemy(other);
        }
    }

    private void RayCheckHitEnemy()
    {
        Vector3 dir = this.transform.position - lastPos;//上一帧位置打向当前帧位置的方向
        Ray ray = new Ray(this.transform.position, dir);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, dir.magnitude, 1 << LayerMask.NameToLayer("Enemy"), QueryTriggerInteraction.UseGlobal))
        {
            HitEnemy(hitInfo.collider);
        }

        lastPos = this.transform.position;
    }

    private void HitEnemy(Collider collider)
    {
        collider.gameObject.GetComponent<EnemyManager>().TakeDamage();
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
