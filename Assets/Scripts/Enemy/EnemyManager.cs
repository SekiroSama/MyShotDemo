using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed = -5f;
    public float hp = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIsWall())
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
        hp--;
        if(hp <= 0)
        {
            Dead();
        }
        Debug.Log("Enemy took damage!");
    }

    private void FlipModel()
    {
        this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }

    private bool CheckIsWall()
    {
        Ray ray = new Ray(this.transform.position, Mathf.Sign(speed) * Vector3.right);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 0.6f, 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Enemy"), QueryTriggerInteraction.UseGlobal))
        {
            Debug.Log("CheckIsWallorEnemy!");
            return true;
        }
        return false;
    }

    private void Dead()
    {
        Destroy(this.gameObject);
        Debug.Log("Enemy died!");
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Ray ray = new Ray(this.transform.position, Mathf.Sign(speed) * Vector3.forward);
    //    Vector3 direction = ray.direction;
    //    Gizmos.DrawRay(this.transform.position, direction * 1.1f);
    //}
}
