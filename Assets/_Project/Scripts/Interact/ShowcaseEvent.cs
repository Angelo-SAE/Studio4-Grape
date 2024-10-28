using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowcaseEvent : MonoBehaviour, IInteractable
{
    [Header("Interact Events")]
    [SerializeField] private UnityEvent interactEvent;
    [SerializeField] private UnityEvent firstEvent;
    [SerializeField] private UnityEvent secondEvent;

    [Header("Timer Variables")]
    [SerializeField] private float firstEventDelay;
    [SerializeField] private float secondEventDelay;

    public bool CheckIfInteractable()
    {
        return true;
    }

    public void Interact()
    {
        interactEvent.Invoke();
        StartCoroutine(PeePeePooPoo());
    }

    public void AltInteract() {}

    private IEnumerator PeePeePooPoo()
    {
        yield return new WaitForSeconds(firstEventDelay);

        firstEvent.Invoke();

        yield return new WaitForSeconds(secondEventDelay);

        secondEvent.Invoke();
    }
}
