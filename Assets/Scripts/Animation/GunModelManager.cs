using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModelManager : MonoBehaviour
{
    public float maxDelayDistrance = 0.2f;

    private float maxDelayDistrance_pow2;
    private Vector3 playerPos;
    private Vector3 lastPlayerPos = Vector3.zero;
    private Vector3 lastOffset = Vector3.zero;
    private Vector3 tatolOffset = Vector3.zero;

    private void Start()
    {
        maxDelayDistrance_pow2 = maxDelayDistrance * maxDelayDistrance;
    }

    private void Update()
    {
        playerPos = GameManager.Instance.player.transform.position;
        lastOffset = GameManager.Instance.player.transform.InverseTransformVector(lastPlayerPos - playerPos) ;
        lastPlayerPos = playerPos;
        if (lastOffset.sqrMagnitude == 0f)
        {
            tatolOffset *= 0.9f;
        }
        else
        {
            tatolOffset += lastOffset;
        }
        if (tatolOffset.sqrMagnitude > maxDelayDistrance_pow2)
        {
            tatolOffset = tatolOffset.normalized * maxDelayDistrance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, tatolOffset, Time.deltaTime * 10f);
    }
}
