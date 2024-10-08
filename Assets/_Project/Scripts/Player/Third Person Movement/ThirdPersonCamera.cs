using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private BoolObject paused;

    [Header("Camera Rotation")]
    [SerializeField] private float cameraSensitivityX;
    [SerializeField] private float cameraSensitivityY;

    private float mouseX;
    private float mouseY;
    private float clampedRotationX;

    [Header("Camera Zoom")]
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private float zoomSensitivity;
    [SerializeField] private float minCameraZoom;
    [SerializeField] private float maxCameraZoom;

    private float mouseScroll;
    private float cameraZoom;


    private void Start()
    {
        transform.rotation = Quaternion.Euler(25, 0, 0);
        cameraZoom = 4;
    }

    private void Update()
    {
        if(!paused.value)
        {
            GetMouseInputs();
            RotateCamera();
            ZoomCamera();
        }
    }

    private void GetMouseInputs()
    {
        mouseX = Input.GetAxis("Mouse X") * cameraSensitivityX * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * cameraSensitivityY * Time.deltaTime;
        mouseScroll = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * Time.deltaTime;

    }

    private void RotateCamera()
    {

        clampedRotationX -= mouseY;
        clampedRotationX = Mathf.Clamp(clampedRotationX, -40, 80);

        transform.rotation = Quaternion.Euler(clampedRotationX, transform.eulerAngles.y + mouseX, transform.eulerAngles.z);
    }

    private void ZoomCamera()
    {
        cameraZoom = Mathf.Clamp(cameraZoom - mouseScroll, minCameraZoom, maxCameraZoom);
        cameraObject.transform.localPosition =  new Vector3(0, 0, -cameraZoom);
    }
}
