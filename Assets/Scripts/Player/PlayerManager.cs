using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform gunPos;
    public GameObject bulletPrefab;
    private Rigidbody rb;
    private CharacterController cc;

    public float speed = 5f;
    public float jumpForce = 5f;

    private GameObject bullet;
    private List<GameObject> bullets = new List<GameObject>();

    private Vector2 moveDir;

    // Start is called before the first frame update
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        cc = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandInput();

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        Move();
        HandGravity();
    }

    Vector3 gravity = new Vector3(0, -9.81f, 0);
    private void HandGravity()
    {
        cc.Move(gravity * Time.deltaTime);
    }

    private void HandInput()
    {
        moveDir.x = Input.GetAxis("Horizontal");
    }

    private void Fire()
    {
        bullet = Instantiate(bulletPrefab);
        bullet.transform.position = new Vector3(gunPos.position.x, gunPos.position.y, 0);
        bullets.Add(bullet);
    }

    private void Move()
    {
        if(moveDir.sqrMagnitude > 0.001)
        {
            cc.Move(moveDir * speed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
