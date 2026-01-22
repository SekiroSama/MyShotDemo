using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform[] gunPoses;
    public GameObject bulletPrefab;
    public GameObject gunFire;
    public GameObject bulletCasePrefab;
    public Transform bulletCaseBornTransform;
    private Rigidbody rb;

    public float speed = 5f;
    public float jumpForce = 5f;
    public float fireRate = 0.5f;
    public float recoilPower = 0.5f;

    private GameObject bullet;
    private List<GameObject> bullets = new List<GameObject>();

    private Vector2 moveDir;
    private bool isJump = false;
    private float fireRateTimer = 0f;
    private bool isFacingRight = true;
    private bool isTakeRecoiling = false;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandHorizontalInput();

        fireRateTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && fireRateTimer >= fireRate)
        {
            Fire();
            fireRateTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && CheckIsGroundedOrEnemy())
        {
            isJump = true;
        }

        Move();
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            Jump();
            isJump = false;
        }

        if (isTakeRecoiling)
        {
            rb.AddForce(-this.transform.right * recoilPower, ForceMode.Impulse);
            isTakeRecoiling = false;
        }

        rb.velocity = new Vector3(speed * moveDir.x, rb.velocity.y, 0);
    }

    private void FlipModel()
    {
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        foreach (Transform gunPos in gunPoses)
        {
            gunPos.Rotate(0, 180f, 0);
        }
    }

    private void HandHorizontalInput()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
    }

    private void Fire()
    {
        foreach (Transform gunPos in gunPoses)
        {
            bullet = Instantiate(bulletPrefab);
            bullet.transform.position = new Vector3(gunPos.position.x, gunPos.position.y, 0);
            bullet.transform.rotation = gunPos.rotation;
            bullets.Add(bullet);
            GenerateBulletCase(gunPos);
        }

        ShowGunFire();
        TakeRecoil();
        GameManager.Instance.cameraManager.isFireShaking = true;
    }

    private void GenerateBulletCase(Transform gunPos)
    {
        GameObject bulletCase = Instantiate(bulletCasePrefab);
        bulletCase.transform.position = bulletCaseBornTransform.position;
        bulletCase.transform.rotation = gunPos.rotation;
        bulletCase.GetComponent<Rigidbody>().AddForce
            ((isFacingRight ? -bulletCaseBornTransform.right : bulletCaseBornTransform.right) * Random.Range(2f, 3f) + bulletCaseBornTransform.up * Random.Range(3f, 5f), ForceMode.Impulse);
    }

    private void TakeRecoil()
    {
        isTakeRecoiling = true;
    }

    private void ShowGunFire()
    {
        gunFire.SetActive(true);
        Invoke("HideGunFire", 0.1f);
    }

    private void HideGunFire()
    {
        gunFire.SetActive(false);
    }

    private void Move()
    {
        if((moveDir.x > 0.01 && !isFacingRight) || (moveDir.x < -0.01 && isFacingRight))
        {
            FlipModel();
            isFacingRight = !isFacingRight;
            GameManager.Instance.cameraManager.offset = new Vector3(-GameManager.Instance.cameraManager.offset.x, GameManager.Instance.cameraManager.offset.y, GameManager.Instance.cameraManager.offset.z);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool CheckIsGroundedOrEnemy()
    {
        Vector3 pos = this.transform.position;
        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.6f))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                EnemyManager enemy = hit.collider.GetComponent<EnemyManager>();
                enemy.TakeDamage();
            }
            return true;
        }

        pos.x += 0.5f;
        ray = new Ray(pos, Vector3.down);
        if (Physics.Raycast(ray, out hit, 0.6f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyManager enemy = hit.collider.GetComponent<EnemyManager>();
                enemy.TakeDamage();
            }
            return true;
        }

        pos.x -= 1f;
        ray = new Ray(pos, Vector3.down);
        if (Physics.Raycast(ray, out hit, 0.6f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyManager enemy = hit.collider.GetComponent<EnemyManager>();
                enemy.TakeDamage();
            }
            return true;
        }
        return false;
    }
}
