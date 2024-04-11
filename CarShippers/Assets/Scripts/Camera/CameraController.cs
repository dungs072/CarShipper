using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float heightSpeed = 2;
    //[SerializeField] private float screenBorderThickness = 10f;
    [SerializeField] private Vector2 screenXLimits = Vector2.zero;
    [SerializeField] private Vector2 screenZLimits = Vector2.zero;
    [SerializeField] private Vector2 screenYLimits = Vector2.zero;
    private Vector2 previousInput;
    private float heightInput;
    private float height = 0f;
    private Controls controls;
    private Coroutine zoomCoroutine;
    private void Awake()
    {
        height = cameraTransform.position.y;
    }
    private void Start()
    {
        controls = new Controls();

        controls.Player.MoveCamera.performed += SetPreviousInput;
        controls.Player.MoveCamera.canceled += SetPreviousInput;

        controls.Player.HeightCamera.performed += SetHeightInput;
        controls.Player.HeightCamera.canceled += SetHeightInput;

        //touch
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
        controls.Enable();
    }
    private void Update()
    {
        if (!Application.isFocused) { return; }
        UpdateCameraPosition();
        UpdateHeightPosition();
    }
    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine);
    }
    private IEnumerator ZoomDetection()
    {
        float previousDistance = 0f;
        float distance = 0f;
        while (true)
        {
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>()
                    , controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
            // detection
            // zoom out
            if (distance > previousDistance)
            {
                heightInput = -60;
                UpdateHeightPosition();
            }
            //zoom in
            else if (distance < previousDistance)
            {
                heightInput = 60;
                UpdateHeightPosition();
            }
            // else
            // {
            //     yield break;
            // }

            previousDistance = distance;
            yield return null;
        }
    }
    private void UpdateHeightPosition()
    {

        height -= heightInput * heightSpeed * Time.deltaTime;
        height = Mathf.Clamp(height, screenYLimits.x, screenYLimits.y);
        cameraTransform.position = new Vector3(cameraTransform.position.x, height, cameraTransform.position.z);
    }
    private void UpdateCameraPosition()
    {
        Vector3 pos = cameraTransform.position;
        if (previousInput == Vector2.zero)
        {
            // Vector3 cursorMovement = Vector3.zero;
            // Vector2 cursorPosition = Mouse.current.position.ReadValue();

            // if (cursorPosition.y >= Screen.height - screenBorderThickness)
            // {
            //     cursorMovement.z += 1;
            // }
            // else if (cursorPosition.y <= screenBorderThickness)
            // {
            //     cursorMovement.z -= 1;
            // }
            // if (cursorPosition.x >= Screen.width - screenBorderThickness)
            // {
            //     cursorMovement.x += 1;
            // }
            // else if (cursorPosition.x <= screenBorderThickness)
            // {
            //     cursorMovement.x -= 1;
            // }

            // pos+=cursorMovement.normalized * speed * Time.deltaTime;
        }
        else
        {
            pos += new Vector3(previousInput.x, 0, previousInput.y) * speed * Time.deltaTime;
        }
        pos.x = Mathf.Clamp(pos.x, screenXLimits.x, screenXLimits.y);
        pos.z = Mathf.Clamp(pos.z, screenZLimits.x, screenZLimits.y);
        cameraTransform.position = pos;
    }
    private void SetPreviousInput(InputAction.CallbackContext ctx)
    {
        previousInput = ctx.ReadValue<Vector2>();
    }
    private void SetHeightInput(InputAction.CallbackContext ctx)
    {
        heightInput = ctx.ReadValue<float>();
    }
    public void SetScreenXLimits(Vector2 screenX)
    {
        screenXLimits = screenX;
    }
    public void SetScreenZLimits(Vector2 screenZ)
    {
        screenZLimits = screenZ;
    }
    public void SetScreenYLimits(Vector2 screenY)
    {
        screenYLimits = screenY;
    }
}
