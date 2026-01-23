using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraFather;
    private Transform playerTransform;
    public float smoothSpeed = 5f;
    public float shakeIntensity = 1f;

    //private Vector3 originalPos;
    public bool isFireShaking = false;
    public bool isEnemydeadShaking = false;

    public Vector3 offset;

    public void Init(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    private void LateUpdate()
    {
        cameraFather.position = Vector3.Lerp(cameraFather.position, new Vector3(playerTransform.position.x, playerTransform.position.y, cameraFather.position.z) + offset, smoothSpeed * Time.deltaTime);

        if (isFireShaking)
        {
            CameraShaking_Fire();
            Invoke("StopCameraShaking_Fire", 0.1f);
        }
        
        if (isEnemydeadShaking)
        {
            CameraShaking_Enemydead();
            Invoke("StopCameraShaking_Enemydead", 0.1f);
        }

        if( !isFireShaking && !isEnemydeadShaking)
        {
            transform.localPosition = Vector3.back * 9;
        }
    }
    private void CameraShaking_Fire()
    {
        transform.localPosition += shakeIntensity * Time.deltaTime * Random.Range(0.5f, 1f) * (GameManager.Instance.player.GetIsFacingRight() ? -Vector3.right : Vector3.right);
    }

    private void StopCameraShaking_Fire()
    {
        isFireShaking = false;
    }

    private void CameraShaking_Enemydead()
    {
        transform.localPosition += shakeIntensity * Time.deltaTime * Random.insideUnitSphere * 4;
    }

    private void StopCameraShaking_Enemydead()
    {
        isEnemydeadShaking = false;
    }
}
