using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed;
    public float range;

    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 2f);
        lastPos = this.transform.position;
        this.transform.Rotate(0, 0, Random.Range(-range, range));
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitEnemy(collision.collider);
            ShowHitEff(collision.contacts[0].point);
            DestroySelf();
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            ShowHitEff(collision.contacts[0].point);
            DestroySelf();
        }
    }

    private void RayCheckHitEnemy()
    {
        Vector3 dir = this.transform.position - lastPos;//上一帧位置打向当前帧位置的方向
        Ray ray = new Ray(this.transform.position, dir);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, dir.magnitude, 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Wall"), QueryTriggerInteraction.UseGlobal))
        {
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                HitEnemy(hitInfo.collider);
                ShowHitEff(hitInfo.point);
                DestroySelf();
            }

            if (hitInfo.collider.gameObject.CompareTag("Wall"))
            {
                ShowHitEff(hitInfo.point);
                DestroySelf();
            }
        }

        lastPos = this.transform.position;
    }

    private void HitEnemy(Collider collider)
    {
        collider.gameObject.GetComponent<EnemyManager>().TakeDamage();
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void ShowHitEff(Vector3 pos)
    {
        GameManager.Instance.ShowHitEffect(pos);
    }
}
