using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        this.transform.Translate(-speed * Time.deltaTime, 0, 0);
    }

    public void TakeDamage()
    {
        Debug.Log("Enemy took damage!");
    }

    private void FlipModel()
    {
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }
}
