using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;

    [Header("Interactable Variables")]
    [SerializeField] public bool hasPopup;

    [Header("Ladder Variables")]
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform topEndPosition;
    [SerializeField] private float ladderHeight;

    public bool CheckIfInteractable()
    {
        return true;
    }

    public void Interact()
    {
        StartLadderClimb();
    }

    public void AltInteract() {}

    private void StartLadderClimb()
    {
        if((transform.position.y + (ladderHeight/2)) - playerObject.value.transform.position.y > 0)
        {
            playerObject.value.GetComponent<ThirdPersonMovement>().StartLadderClimb(new Vector3(playerPosition.position.x, transform.position.y + 0.4f, playerPosition.position.z), topEndPosition.position, playerPosition.localEulerAngles, transform.position.y + ladderHeight - 1.8f, transform.position.y + 0.2f, false);
        } else {
            playerObject.value.GetComponent<ThirdPersonMovement>().StartLadderClimb(new Vector3(playerPosition.position.x, transform.position.y + ladderHeight - 3f, playerPosition.position.z), topEndPosition.position, playerPosition.localEulerAngles, transform.position.y + ladderHeight - 1.8f, transform.position.y + 0.2f, true);
        }
    }


}
