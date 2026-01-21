using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate_UpAndDown : MonoBehaviour
{
    public float speed = 50f;
    private float timer = 0f;
    public float qunzhong = 0.5f;
    void Update()
    {
        this.transform.Translate(transform.up * Mathf.Sin(timer) * Time.deltaTime * qunzhong);
        timer += Time.deltaTime * speed;
    }
}
