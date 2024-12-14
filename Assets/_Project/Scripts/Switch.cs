using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] private IntObject switchCount;
    [SerializeField] private Animator animator;
    private bool isInteractable;

    private void Start()
    {
        isInteractable = true;
    }

    public bool CheckIfInteractable()
    {
        return isInteractable;
    }

    public void Interact()
    {
        isInteractable = false;
        switchCount.value++;
        animator.Play("Flip Switch");
    }

    public void AltInteract(){}
}
