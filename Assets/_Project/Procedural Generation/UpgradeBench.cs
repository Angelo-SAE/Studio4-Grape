using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBench : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObjectObject upgradeMenuObject;
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
        upgradeMenuObject.value.SetActive(true);
    }

    public void AltInteract(){}
}
