using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    private Vector3 playerCameraVecto3;

    public float smoothSpeed = 0.125f;

    public Vector3 offset;
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        playerCameraVecto3 = PlayerMovement.instance.transform.position - transform.position;
    }

    void LateUpdate()
    {
        if (PlayerChangePos.instance.isWin)
        {
            Vector3 desiredPos = PlayerMovement.instance.transform.position + offset;
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothPos;

            transform.LookAt(PlayerMovement.instance.transform.position);
            return;
        }
        transform.position = PlayerMovement.instance.transform.position - playerCameraVecto3;

    }

}
    
