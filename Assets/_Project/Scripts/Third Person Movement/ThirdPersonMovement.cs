using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private float movementSpeed;


    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 movementDirection;
    private Rigidbody rb;
    private bool stopped;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;

    private float rotationAngle;
    private bool isRotating;

    [Header("Ground Detection")]
    [SerializeField] private string groundTag;
    [SerializeField] private float objectHeight;
    [SerializeField] private float maxSlopeAngle;

    private bool grounded;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * ((objectHeight * 0.5f )+ 0.1f));
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //temp for now
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(grounded)
        {
            GetKeyInputs();
            CheckForMove();
        } else {
            CheckForGrounded();
        }

        if(isRotating)
        {
            RotateObject();
        }

    }

    private void GetKeyInputs()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
    }

    private void CheckForMove()
    {
        if(horizontalMovement != 0 || verticalMovement != 0)
        {
            if(grounded && CheckGroundAngle())
            {
                stopped = false;
                GetRotationAngle();
                MoveObject();
            }
        } else if(!stopped)
        {
            StopObject();
        }
    }

    private void GetRotationAngle()
    {
        switch(verticalMovement)
        {
            case(1):
            switch(horizontalMovement)
            {
                case(1):
                rotationAngle = 45;
                break;
                case(-1):
                rotationAngle = -45;
                break;
                case(0):
                rotationAngle = 0;
                break;
            }
            break;

            case(-1):
            switch(horizontalMovement)
            {
                case(1):
                rotationAngle = 135;
                break;
                case(-1):
                rotationAngle = -135;
                break;
                case(0):
                rotationAngle = 180;
                break;
            }
            break;

            case(0):
            switch(horizontalMovement)
            {
                case(1):
                rotationAngle = 90;
                break;
                case(-1):
                rotationAngle = -90;
                break;
            }
            break;
        }
        if(verticalMovement == 0)
        {
            if(horizontalMovement == 1)
            {
                rotationAngle = 90;
            } else if(horizontalMovement == -1)
            {
                rotationAngle = -90;
            }
        }

        isRotating = true;
    }

    private void RotateObject()
    {
        /*
        if(objectToRotate.transform.eulerAngles.y < rotationAngle)
        {
            objectToRotate.transform.rotation = Quaternion.Euler(0, objectToRotate.transform.localEulerAngles.y + (rotationSpeed * Time.deltaTime), 0);
        } else if(objectToRotate.transform.eulerAngles.y > rotationAngle)
        {
            objectToRotate.transform.rotation = Quaternion.Euler(0, objectToRotate.transform.localEulerAngles.y - (rotationSpeed * Time.deltaTime), 0);
        } else {
            isRotating = false;
        }
        */

        objectToRotate.transform.rotation = Quaternion.Euler(0, cameraHolder.transform.localEulerAngles.y + rotationAngle, 0);
    }

    private void MoveObject()
    {
        rb.velocity = movementDirection * movementSpeed;
    }

    private void StopObject()
    {
        stopped = true;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    private void CheckForGrounded()
    {
        RaycastHit groundHit;
        if(Physics.Raycast(transform.position, Vector3.down, out groundHit, objectHeight * 0.5f + 0.1f))
        {
            if(groundHit.transform.tag == groundTag)
            {
                grounded = true;
            }
        }
    }

    private bool CheckGroundAngle()
    {
        RaycastHit groundHit;
        if(Physics.Raycast(transform.position, Vector3.down, out groundHit, objectHeight * 0.5f + 0.8f))
        {
            float groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
            if(groundAngle < maxSlopeAngle)
            {
                GetMovementDirection(groundHit.normal);
                return true;
            }
        }
        return false;
    }

    private void GetMovementDirection(Vector3 groundNormal)
    {
        movementDirection = Vector3.ProjectOnPlane(objectToRotate.transform.forward, groundNormal);
    }
}
