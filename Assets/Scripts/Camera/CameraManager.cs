using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform playerTransform;

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
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, this.transform.position.z);
    }
}
