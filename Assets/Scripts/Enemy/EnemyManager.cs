using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed = -5f;
    public float hp = 2f;
    public GameObject deadBodyPrefab;
    public GameObject bigBoomPrefab;

    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = this.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIsWallorEnemy())
        {
            FlipModel();
            speed = -speed;
        }

        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy hit Player!");
        }
    }

    private void Move()
    {
        this.transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    public void TakeDamage()
    {
        if(hp <= 0) return;

        hp--;
        if(hp <= 0)
        {
            Dead();
        }
        TakeDamageEffOn();
        GameManager.Instance.TimeStop(0.02f);
        Debug.Log("Enemy took damage!");
    }

    private void TakeDamageEffOn()
    {
        material.color = Color.red;
        Invoke("TakeDamageEffOff", 0.1f);
    }

    private void TakeDamageEffOff()
    {
        material.color = Color.black;
    }

    private void FlipModel()
    {
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }

    private bool CheckIsWallorEnemy()
    {
        Ray ray = new Ray(this.transform.position, Mathf.Sign(speed) * Vector3.right);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 0.6f, 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Enemy"), QueryTriggerInteraction.UseGlobal))
        {
            //Debug.Log("CheckIsWallorEnemy!");
            return true;
        }
        return false;
    }

    private void Dead()
    {
        GenateDeadBody();
        GameManager.Instance.cameraManager.isEnemydeadShaking = true;
        DeadRangeBigBoom();
        Destroy(this.gameObject);
    }

    private void DeadRangeBigBoom()
    {
        float random = Random.Range(0f, 0.4f);
        if(random >= 0.3f)
        {
            GameObject deadRangeBigBoom = Instantiate(bigBoomPrefab);
            deadRangeBigBoom.transform.position = this.transform.position;
        }
    }
    
    private void GenateDeadBody()
    {
        GameObject deadBody = Instantiate(deadBodyPrefab);
        deadBody.transform.position = this.transform.position + Vector3.down * 0.35f;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Ray ray = new Ray(this.transform.position, Mathf.Sign(speed) * Vector3.forward);
    //    Vector3 direction = ray.direction;
    //    Gizmos.DrawRay(this.transform.position, direction * 1.1f);
    //}
}
