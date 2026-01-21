using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform playerTransform;
    public float smoothSpeed = 5f;

    public void Init(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(playerTransform.position.x, playerTransform.position.y, this.transform.position.z), smoothSpeed * Time.deltaTime);
    }
}
