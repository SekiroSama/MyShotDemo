using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform gunPos;
    public GameObject bulletPrefab;

    private GameObject bullet;
    private List<GameObject> bullets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        bullet = Instantiate(bulletPrefab);
        bullet.transform.position = new Vector3(gunPos.position.x, gunPos.position.y, 0);
        bullets.Add(bullet);
    }
}
