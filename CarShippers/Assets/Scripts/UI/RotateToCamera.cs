using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 lookDirection = mainCamera.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}
