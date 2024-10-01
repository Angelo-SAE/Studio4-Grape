using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("KeyCodes")]
    [SerializeField] private KeyCode sprintKey;
    [SerializeField] private KeyCode jumpKey;

    [Header("Movement")]
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintSpeed;

    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 movementDirection;
    private Rigidbody rb;
    private bool sprinting;
    private bool stopped;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDelay;

    private float currentDelay;
    private bool isAbleToJump;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;

    private float rotationAngle;
    private bool isRotating;

    [Header("Ground Detection")]
    [SerializeField] private string groundTag;
    [SerializeField] private float objectHeight;
    [SerializeField] private float maxSlopeAngle;

    private bool grounded;

    //Player Animation Controller
    [SerializeField] private Animator animator;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.down * ((objectHeight * 0.5f )+ 0.1f));
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
        grounded = CheckForGrounded();

        GetKeyInputs();
        CheckForMove();

        if(isRotating)
        {
            RotateObject();
        }

        if(!isAbleToJump)
        {
            currentDelay += Time.deltaTime;
            if(currentDelay >= jumpDelay)
            {
                isAbleToJump = true;
            }
        }


    }

    private void GetKeyInputs()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(sprintKey))
        {
            sprinting = true;
        } else {
            sprinting = false;
        }
        if(Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }

    private void CheckForMove()
    {
        if(horizontalMovement != 0 || verticalMovement != 0)
        {
            stopped = false;
            GetRotationAngle();
            MoveObject();

            animator.SetBool("PlayerMoving", true); //Added the Start of Player Moving Animation

        } else if(!stopped && grounded)
        {
            StopObject();

            //Somehow This is not Updating the Animator Controller
            animator.SetBool("PlayerMoving", false); //Added the End of Player Moving Animation
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
        Vector3 movementVelocity = Vector3.zero;
        if(sprinting)
        {
            movementVelocity = objectToRotate.transform.forward * sprintSpeed;
        } else {
            movementVelocity = objectToRotate.transform.forward * movementSpeed;
        }
        rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);

    }

    private void Jump()
    {
        if(grounded && isAbleToJump)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            grounded = false;
            isAbleToJump = false;
            currentDelay = 0;
        }
    }

    private void StopObject()
    {
        stopped = true;
        rb.velocity = new Vector3(0, 0, 0);
    }

    private bool CheckForGrounded()
    {
        RaycastHit groundHit;
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.down, out groundHit, objectHeight * 0.5f + 0.1f))
        {
            if(groundHit.transform.tag == groundTag)
            {
                return true;
            }
        }
        return false;
    }

    /*
    private bool CheckGroundAngle()
    {
        RaycastHit groundHit;
        if(Physics.Raycast(objectToRotate.transform.position, Vector3.down, out groundHit, objectHeight * 0.5f + 0.8f))
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
    */
}
