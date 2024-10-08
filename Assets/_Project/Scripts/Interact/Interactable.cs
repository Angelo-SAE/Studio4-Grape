using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool CheckIfInteractable();
    void Interact();
    void AltInteract();
}
