using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private KeyBindingsObject keyBindings;
    [SerializeField] private BoolObject paused;
    [SerializeField] private BoolObject canInteract;
    [SerializeField] private BoolObject playerIsShooting;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [Header("PlayerStats")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Movement")]
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private GameObject objectToRotate;
    [SerializeField] private GameObject movementDirectionObject;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private Rigidbody rb;


    private Vector3 movementDirection;
    private bool sprinting;
    private bool stopped;

    //Inputs
    private float horizontalPos;
    private float horizontalNeg;
    private float verticalPos;
    private float verticalNeg;
    private float horizontalMovement;
    private float verticalMovement;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDelay;

    private float currentDelay;
    private bool isAbleToJump;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private GameObject playerCollider;

    private bool crouching;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;

    private float rotationAngle;
    private bool isRotating;

    [Header("Ground Detection")]
    [SerializeField] private string groundTag;
    [SerializeField] private float objectHeight;
    [SerializeField] private float maxSlopeAngle;

    private bool grounded;

    [Header("Ladder Movement")]
    [SerializeField] private float climbingSpeed;
    [SerializeField] private float climbingSprintSpeed;

    private bool isClimbing;
    private float topEndHeight;
    private float bottomEndHeight;
    private Vector3 topEndPosition;
    private Vector3 topEndRotation;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.down * ((objectHeight * 0.5f )+ 0.1f));
    }

    private void Start()
    {
        //temp for now
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerIsShooting.value = false;
    }

    private void Update()
    {
        if(!paused.value)
        {
            GetKeyInputs();

            if(isClimbing)
            {
                ClimbLadder();
            } else {
                grounded = CheckForGrounded();

                CheckForMove();

                if(isRotating)
                {
                    RotateObject();
                }

                animator.SetBool("Shooting", playerIsShooting.value); //Animation
                if(playerIsShooting.value)
                {
                    objectToRotate.transform.rotation = Quaternion.Euler(0, cameraHolder.transform.localEulerAngles.y, 0);
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
        } else if(!stopped)
        {
            StopObject();
        }
    }

    private void GetKeyInputs()
    {
        GetDirectionInputs();

        horizontalMovement = horizontalPos + horizontalNeg;
        verticalMovement = verticalPos + verticalNeg;

        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(horizontalMovement);
            Debug.Log(verticalMovement);
        }

        animator.SetFloat("PositionX", horizontalMovement); //Animation
        animator.SetFloat("PositionY", verticalMovement); //Animation

        if(Input.GetKey(keyBindings.sprint))
        {
            animator.SetFloat("MovementSpeed", 1.3f);
            sprinting = true;
        } else {
            animator.SetFloat("MovementSpeed", 1f);
            sprinting = false;
        }
        if(Input.GetKeyDown(keyBindings.jump))
        {
            Jump();
            EndLadderClimb();
        }
        if(Input.GetKey(keyBindings.crouch) && !isClimbing)
        {
            if(!crouching)
            {
                crouching = true;
                animator.Play("Crouch");
                animator.SetBool("Crouching", true);
                playerCollider.transform.localScale = new Vector3(1f, 0.5f, 1f); //currently y 0.77 is the right amount
            }
        } else if(crouching)
        {
            playerCollider.transform.localScale = new Vector3(1f, 1f, 1f);
            crouching = false;
            animator.SetBool("Crouching", false);
        }
    }

    private void GetDirectionInputs()
    {
        if(Input.GetKey(keyBindings.right))
        {
            horizontalPos = 1;
        } else {
            horizontalPos = 0;
        }
        if(Input.GetKey(keyBindings.left))
        {
            horizontalNeg = -1;
        } else {
            horizontalNeg = 0;
        }

        if(Input.GetKey(keyBindings.forward))
        {
            verticalPos = 1;
        } else {
            verticalPos = 0;
        }
        if(Input.GetKey(keyBindings.backward))
        {
            verticalNeg = -1;
        } else {
            verticalNeg = 0;
        }
    }

    private void CheckForMove()
    {
        if(horizontalMovement != 0 || verticalMovement != 0)
        {
            stopped = false;
            GetRotationAngle();
            MoveObject();

            animator.SetBool("PlayerMoving", true); //Animation

        } else if(!stopped && grounded)
        {
            StopObject();
            isRotating = false;

            animator.SetBool("PlayerMoving", false); //Animation
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
        if(!playerIsShooting.value)
        {
            objectToRotate.transform.rotation = Quaternion.Euler(0, cameraHolder.transform.localEulerAngles.y + rotationAngle, 0);
        }
        movementDirectionObject.transform.rotation = Quaternion.Euler(0, cameraHolder.transform.localEulerAngles.y + rotationAngle, 0);
    }

    private void MoveObject()
    {
        Vector3 movementVelocity = Vector3.zero;
        if(crouching)
        {
            movementVelocity = movementDirectionObject.transform.forward * crouchSpeed * playerStats.MovementSpeed;
        } else if(sprinting)
        {
            movementVelocity = movementDirectionObject.transform.forward * sprintSpeed * playerStats.MovementSpeed;
        } else {
            movementVelocity = movementDirectionObject.transform.forward * movementSpeed * playerStats.MovementSpeed;
        }
        rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);

    }

    private void Jump()
    {
        if(grounded || isClimbing && isAbleToJump )
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            grounded = false;
            isAbleToJump = false;
            currentDelay = 0;
            animator.SetTrigger("Jump"); //Animation
        }
    }

    private void StopObject()
    {
        stopped = true;
        horizontalPos = 0;
        horizontalNeg = 0;
        verticalPos = 0;
        verticalNeg = 0;
        animator.SetBool("PlayerMoving", false); //Animation
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

    public void StartLadderClimb(Vector3 sPosition, Vector3 tEndPosition, Vector3 rotation, Vector3 tEndRotation, float tEndHeight, float bEndHeight, bool top)
    {
        // Disable unnecessary player components
        playerCollider.SetActive(false);
        rb.useGravity = false;
        StopObject();
        canInteract.value = false;

        // Set ladder end positions and rotations
        topEndHeight = tEndHeight;
        bottomEndHeight = bEndHeight;
        topEndPosition = tEndPosition;
        topEndRotation = tEndRotation;

        // Align player to ladder starting position
        if (top)
        {
            transform.position = new Vector3(sPosition.x, Mathf.Max(transform.position.y, sPosition.y), sPosition.z);
        }
        else
        {
            transform.position = new Vector3(sPosition.x, Mathf.Min(transform.position.y, sPosition.y), sPosition.z);
        }

        // Align player rotation to ladder
        objectToRotate.transform.eulerAngles = rotation;

        // Start ladder climb animation
        animator.SetBool("ClimbingLadder", true); // Use a Bool to transition into climbing states
        animator.Play("Ladder Idle");

        isClimbing = true; // Set climbing state
    }

    private void ClimbLadder()
    {
        // Determine movement direction and update animation speed
        if (Input.GetKey(keyBindings.forward))
        {
            animator.SetFloat("ClimbingSpeed", 1f); // Positive speed for climbing up
            transform.position += Vector3.up * climbingSpeed * playerStats.MovementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(keyBindings.backward))
        {
            animator.SetFloat("ClimbingSpeed", -1f); // Negative speed for climbing down
            transform.position -= Vector3.up * climbingSpeed * Time.deltaTime;
        }
        else
        {
            animator.SetFloat("ClimbingSpeed", 0f); // Idle on the ladder
        }

        // Check if player reaches ladder ends
        if (transform.position.y >= topEndHeight)
        {
            transform.position = topEndPosition;
            objectToRotate.transform.eulerAngles = topEndRotation;
            cameraHolder.transform.eulerAngles = topEndRotation;
            EndLadderClimb();
        }
        else if (transform.position.y <= bottomEndHeight)
        {
            EndLadderClimb();
        }
    }

    private void EndLadderClimb()
    {
        // Re-enable player components
        playerCollider.SetActive(true);
        rb.useGravity = true;
        canInteract.value = true;

        // Reset Animator state
        animator.SetBool("ClimbingLadder", false);

        isClimbing = false; // End climbing state
    }
}
