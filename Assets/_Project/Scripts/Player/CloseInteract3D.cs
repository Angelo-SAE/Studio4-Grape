using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseInteract3D : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private BoolObject canInteract;
    [SerializeField] private BoolObject paused;


    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private KeyCode altInteractKey;


    [Header("Interact Detection")]
    [SerializeField] private GameObject objectDirection; //find better name
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float interactableRange;
    [SerializeField] private float interactableHeight;
    [SerializeField] private float interactableAngle;
    [SerializeField] private float boxHeight;

    private Collider[] interactableObjects;
    private Collider closestInteractable;
    private Collider closest;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + boxHeight, transform.position.z), new Vector3(interactableRange * 2, interactableHeight * 2, interactableRange * 2));
    }

    private void Update()
    {
        if(canInteract.value && !paused.value)
        {
            CheckForInteractable();

            if(Input.GetKeyDown(interactKey))
            {
                Interact();
            }

            if(Input.GetKeyDown(altInteractKey))
            {
                AltInteract();
            }
        }
    }

    private void Interact()
    {
        if(closestInteractable is not null)
        {
            closestInteractable.GetComponent<IInteractable>().Interact();
        }
    }

    private void AltInteract()
    {
        if(closestInteractable is not null)
        {
            closestInteractable.GetComponent<IInteractable>().AltInteract();
        }
    }

    private void CheckForInteractable() //Re-enable outline functions if you want to use a shader to outline what the player can currently interact with
    {
        interactableObjects = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + boxHeight, transform.position.z), new Vector3(interactableRange , interactableHeight, interactableRange), Quaternion.identity, interactLayer);
        if(interactableObjects.Length != 0)
        {
            Collider newClosest = GetClosestInteractable();
            if(closestInteractable != newClosest)
            {
                if(closestInteractable is not null)
                {
                    //RevertOutline();
                }
                closestInteractable = newClosest;
                if(newClosest is not null)
                {
                    //OutlineClosest();
                }
            }
        } else {
            if(closestInteractable is not null)
            {
                //RevertOutline();
                closestInteractable = null;
            }
        }
    }

    private Collider GetClosestInteractable()
    {
        closest = null;
        int startingPoint = 0;

        for(startingPoint = 0; startingPoint < interactableObjects.Length; startingPoint++)
        {
            if(CheckAngle(interactableObjects[startingPoint].transform.position))
            {
                closest = interactableObjects[startingPoint];
                break;
            }
        }
        for(int a = startingPoint + 1; a < interactableObjects.Length; a++)
        {
            if(Vector3.Distance(interactableObjects[a].transform.position, transform.position) < Vector3.Distance(closest.transform.position, transform.position))
            {
                if(CheckAngle(interactableObjects[a].transform.position))
                {
                    closest = interactableObjects[a];
                }
            }
        }
        return closest;
    }

    private bool CheckAngle(Vector3 objPosition)
    {
        Vector3 tempVector = objPosition - objectDirection.transform.position;
        if(Vector3.Angle(objectDirection.transform.forward, new Vector3(tempVector.x, 0f, tempVector.z)) <= interactableAngle)
        {
            return true;
        }
        return false;
    }

    //If you don't want outlines specifically then you can change these functions and names accordingly to have them do what ever.
    //An example of a replacemet would be to just have a UI pop up that lets they player know that they can interact.

    /*
    private void RevertOutline()
    {
        Material objMaterial = closestInteractable.GetComponent<Renderer>().material;
        objMaterial.SetFloat("_OutlineSize", 0f); //Change to fit what is required for your shader
    }

    private void OutlineClosest()
    {
        Material objMaterial = closestInteractable.GetComponent<Renderer>().material;
        objMaterial.SetFloat("_OutlineSize", 0.02f); //Change to fit what is required for your shader
    }
    */
}
