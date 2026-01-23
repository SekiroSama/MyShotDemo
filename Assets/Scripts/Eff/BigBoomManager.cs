using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoomManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 0.25f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyManager>().TakeDamage();
            //Debug.Log("BigBoom hit Enemy!");
        }
    }
}
