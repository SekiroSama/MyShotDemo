using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate_UpAndDown : MonoBehaviour
{
    public float speed = 50f;
    private float timer = 0f;
    public float qunzhong = 0.5f;

    private Vector3 startLocalPos;
    private void Start()
    {
        startLocalPos = this.transform.localPosition;
    }
    void Update()
    {
        this.transform.localPosition = startLocalPos + transform.up * Mathf.Sin(timer) * Time.deltaTime * qunzhong;
        //this.transform.Translate(transform.up * Mathf.Sin(timer) * Time.deltaTime * qunzhong);
        timer += Time.deltaTime * speed;
    }
}
