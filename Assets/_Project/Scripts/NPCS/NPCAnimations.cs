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



    private void Start()
    {
        currentAnimationCycle = animationCycles[Random.Range(0, animationCycles.Length)];
        PlayNextAnimation();
    }

    private void PlayNextAnimation()
    {
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        bool rotating = true;
        Vector3 currentRotation = transform.eulerAngles;
        float lerpValue = 0;
        Vector3 endRotation = currentAnimationCycle.animationRotation[currentAnimation];
        if(currentAnimationCycle.includeMovement[currentAnimation])
        {
            endRotation.y = transform.eulerAngles.y + Vector3.Angle(transform.forward, currentAnimationCycle.movementCordinants[currentMovement] - transform.position); //can maybe clean this up to work a little differently but I think it works fine for now
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

        if(currentAnimationCycle.includeMovement[currentAnimation])
        {
            bool moving = true;
            while(moving)
            {
                yield return 0;
                transform.position = Vector3.MoveTowards(transform.position, currentAnimationCycle.movementCordinants[currentMovement], 1f * Time.deltaTime);
                if(transform.position == currentAnimationCycle.movementCordinants[currentMovement])
                {
                    currentMovement++;
                    moving = false;
                }
            }
        } else {
            yield return new WaitForSeconds(currentAnimationCycle.animationDelay[currentAnimation]);
        }

        GetNextAnimation();
    }

    private void GetNextAnimation()
    {
        currentAnimation++;
        if(currentAnimation == currentAnimationCycle.animations.Length)
        {
            if(currentAnimationCycle.loopCycle)
            {
                currentMovement = 0;
                currentAnimation = 0;
                PlayNextAnimation();
            }
        } else {
            PlayNextAnimation();
        }
    }

    public void PauseAnimation()
    {

    }

    public void StartAnimation()
    {

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
