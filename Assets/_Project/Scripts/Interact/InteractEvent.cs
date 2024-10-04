using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour, IInteractable
{
    [Header("Interact Events")]
    [SerializeField] private UnityEvent interactEvent;
    [SerializeField] private UnityEvent altInteractEvent;

    public void Interact()
    {
        interactEvent.Invoke();
    }

    public void AltInteract()
    {
        altInteractEvent.Invoke();
    }
}
