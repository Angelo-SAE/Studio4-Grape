using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimations : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private NPCAnimationCycle[] animationCycles;

    private NPCAnimationCycle currentAnimationCycle;
    private int currentAnimation;
    private int currentMovement;

    private bool playAnimation;
    private float currentDelay;
    private bool moving;


    private void Start()
    {
        currentAnimationCycle = animationCycles[Random.Range(0, animationCycles.Length)];
        StartCoroutine(RotateNPC());
    }

    private void Update()
    {
        if(playAnimation)
        {
            PlayAnimation();
        }
    }

    private IEnumerator RotateNPC()
    {
        bool rotating = true;
        Vector3 currentRotation = transform.eulerAngles;
        float lerpValue = 0;
        Vector3 endRotation = currentAnimationCycle.animationRotation[currentAnimation]; //check x vectyor if positive means positive if negative means negativbe comparted to transform forward
        //then check if z is negative or piositive to know which 90 degrees
        if(currentAnimationCycle.includeMovement[currentAnimation])
        {
            endRotation.y = transform.eulerAngles.y + Vector3.Angle(transform.forward, currentAnimationCycle.movementCordinants[currentMovement] - transform.position);
        }
        while(rotating)
        {
            yield return 0;
            lerpValue += 8f * Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(currentRotation, endRotation, lerpValue);
            if(lerpValue >= 1)
            {
                rotating = false;
            }
        }

        animator.Play(currentAnimationCycle.animations[currentAnimation]);
        moving = currentAnimationCycle.includeMovement[currentAnimation];
        playAnimation = true;
    }

    private void PlayAnimation()
    {
        if(moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentAnimationCycle.movementCordinants[currentMovement], 1f * Time.deltaTime);
            if(transform.position == currentAnimationCycle.movementCordinants[currentMovement])
            {
                currentMovement++;
                GetNextAnimation();
            }
        } else {
            currentDelay += Time.deltaTime;
            if(currentDelay >= currentAnimationCycle.animationDelay[currentAnimation])
            {
                currentDelay = 0;
                GetNextAnimation();
            }
        }
    }

    private void GetNextAnimation()
    {
        playAnimation = false;
        currentAnimation++;
        if(currentAnimation == currentAnimationCycle.animations.Length)
        {
            if(currentAnimationCycle.loopCycle)
            {
                currentMovement = 0;
                currentAnimation = 0;
                StartCoroutine(RotateNPC());
            }
        } else {
            StartCoroutine(RotateNPC());
        }
    }

    public void PauseAnimation()
    {
        playAnimation = false;
    }

    public void StartAnimation()
    {
        StartCoroutine(RotateNPC());
    }
}

[System.Serializable]
public class NPCAnimationCycle
{
    [Header("Other")]
    public bool loopCycle;

    [Header("Position")]
    public Vector3 startingPosition;

    [Header("Animation")]
    public string[] animations;
    public float[] animationDelay;
    public bool[] includeMovement;

    [Header("Movement During Animations")]
    public Vector3[] animationRotation;
    public Vector3[] movementCordinants;
}
