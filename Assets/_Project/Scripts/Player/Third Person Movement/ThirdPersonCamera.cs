using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;
    [SerializeField] private FloatObject cameraSensitivity;
    [SerializeField] private BoolObject paused;

    [Header("Camera Height")]
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;

    [Header("Camera Rotation")]
    [SerializeField] private float setCameraSensitivity;

    private float mouseX;
    private float mouseY;
    private float clampedRotationX;

    [Header("Camera Zoom")]
    [SerializeField] private LayerMask ignorePlayer;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private float checkLength;
    [SerializeField] private float checkLengthSides;
    [SerializeField] private float zoomSensitivity;
    [SerializeField] private float minCameraZoom;
    [SerializeField] private float maxCameraZoom;

    private float cameraZoom;
    private bool canZoomOut;

    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(cameraObject.transform.position, cameraObject.transform.position + (cameraObject.transform.forward * -1f) * checkLength);
        Gizmos.DrawLine(cameraObject.transform.position, cameraObject.transform.position + (cameraObject.transform.right) * checkLengthSides);
        Gizmos.DrawLine(cameraObject.transform.position, cameraObject.transform.position + (cameraObject.transform.right * -1f) * checkLengthSides);
        Gizmos.DrawLine(cameraObject.transform.position, cameraObject.transform.position + (cameraObject.transform.up) * checkLengthSides);
        Gizmos.DrawLine(cameraObject.transform.position, cameraObject.transform.position + (cameraObject.transform.up * -1f) * checkLengthSides);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(cameraObject.transform.position, new Vector3(playerObject.value.transform.position.x, playerObject.value.transform.position.y + 1.6f, playerObject.value.transform.position.z));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(cameraObject.transform.position + ((cameraObject.transform.forward * -1) * 0.3f), new Vector3(checkLengthSides + checkLengthSides + (checkLengthSides / 2), checkLengthSides + checkLengthSides + (checkLengthSides / 2), checkLength + (checkLength )));
    }
    */

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
            CheckForObjectsBehind();
            ZoomCamera();
            SetCameraHeight();
        }
    }

    private void GetMouseInputs()
    {
        //mouseX = Input.GetAxis("Mouse X") * setCameraSensitivity *  cameraSensitivity.value * Time.deltaTime;
        //mouseY = Input.GetAxis("Mouse Y") * setCameraSensitivity *  cameraSensitivity.value * Time.deltaTime;

        mouseX = Input.GetAxis("Mouse X")  * cameraSensitivity.value;
        mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity.value;
    }

    private void RotateCamera()
    {

        clampedRotationX -= mouseY;
        clampedRotationX = Mathf.Clamp(clampedRotationX, -50, 70);

        transform.rotation = Quaternion.Euler(clampedRotationX, transform.eulerAngles.y + mouseX, transform.eulerAngles.z);
    }

    private void CheckForObjectsBehind()
    {
        RaycastHit hit;
        if(Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward * -1f, out hit, checkLength, ignorePlayer))
        {
            float amount = Vector3.Distance(hit.point, cameraObject.transform.position) / checkLength;
            cameraZoom -= 0.5f * Mathf.Lerp(1f, 0f, amount);
            canZoomOut = false;
        }
        if(Physics.Raycast(cameraObject.transform.position, cameraObject.transform.right, out hit, checkLengthSides, ignorePlayer))
        {
            float amount = Vector3.Distance(hit.point, cameraObject.transform.position) / checkLengthSides;
            cameraZoom -= 0.5f * Mathf.Lerp(1f, 0f, amount);
            canZoomOut = false;
        }
        if(Physics.Raycast(cameraObject.transform.position, cameraObject.transform.right * -1f, out hit, checkLengthSides, ignorePlayer))
        {
            float amount = Vector3.Distance(hit.point, cameraObject.transform.position) / checkLengthSides;
            cameraZoom -= 0.5f * Mathf.Lerp(1f, 0f, amount);
            canZoomOut = false;
        }
        if(Physics.Raycast(cameraObject.transform.position, cameraObject.transform.up, out hit, checkLengthSides, ignorePlayer))
        {
            float amount = Vector3.Distance(hit.point, cameraObject.transform.position) / checkLengthSides;
            cameraZoom -= 0.5f * Mathf.Lerp(1f, 0f, amount);
            canZoomOut = false;
        }
        if(Physics.Raycast(cameraObject.transform.position, cameraObject.transform.up * -1f, out hit, checkLengthSides, ignorePlayer))
        {
            float amount = Vector3.Distance(hit.point, cameraObject.transform.position) / checkLengthSides;
            cameraZoom -= 0.5f * Mathf.Lerp(1f, 0f, amount);
            canZoomOut = false;
        }
        if(Physics.Raycast(cameraObject.transform.position, new Vector3(playerObject.value.transform.position.x, playerObject.value.transform.position.y + 1.6f, playerObject.value.transform.position.z) - cameraObject.transform.position, out hit))
        {
            if(hit.collider.tag != "Player")
            {
                cameraZoom -= 0.3f;
                canZoomOut = false;
            }
        }

        //need to fix
        //if(Physics.Raycast(new Vector3(playerObject.value.transform.position.x, playerObject.value.transform.position.y + 1.6f, playerObject.value.transform.position.z), cameraObject.transform.position - new Vector3(playerObject.value.transform.position.x, playerObject.value.transform.position.y + 1.6f, playerObject.value.transform.position.z), out hit))
        //{
        //    if(hit.collider.tag != "Camera")
        //    {
        //        cameraZoom -= 1f;
        //        canZoomOut = false;
        //    }
        //}

    }

    private void ZoomCamera()
    {
        if(canZoomOut)
        {
            if(!Physics.BoxCast(cameraObject.transform.position, new Vector3(checkLengthSides * 1.6f, checkLengthSides * 1.6f, 0.1f),  cameraObject.transform.forward * -1f, transform.rotation, 1.5f, ignorePlayer))
            {
                cameraZoom += 0.04f * Mathf.Lerp(1, 0, cameraZoom/maxCameraZoom);
            }

        } else {
            canZoomOut = true;
        }
        cameraZoom = Mathf.Clamp(cameraZoom, minCameraZoom, maxCameraZoom);
        cameraObject.transform.localPosition =  new Vector3(0, 0, -cameraZoom);
    }

    private void SetCameraHeight()
    {
        float cameraHeight = Mathf.Lerp(maxHeight, minHeight, cameraZoom/maxCameraZoom);
        transform.localPosition = new Vector3(0, cameraHeight, 0);
    }
}
