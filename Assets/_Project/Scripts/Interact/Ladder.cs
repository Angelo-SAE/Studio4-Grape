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
    [SerializeField] private float bottomStartHeight;
    [SerializeField] private float topStartHeight;

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
            //Debug.Log(transform.position.y + 0.1f);
            //Debug.Log(transform.position.y + bottomStartHeight);
            //Debug.Log(transform.position.y + ladderHeight - topStartHeight + 0.2f);
            playerObject.value.GetComponent<ThirdPersonMovement>().StartLadderClimb(new Vector3(playerPosition.position.x, transform.position.y + bottomStartHeight, playerPosition.position.z), topEndPosition.position, playerPosition.localEulerAngles, topEndPosition.localEulerAngles, transform.position.y + ladderHeight - topStartHeight + 0.2f, transform.position.y + 0.1f, false);
        } else {
            playerObject.value.GetComponent<ThirdPersonMovement>().StartLadderClimb(new Vector3(playerPosition.position.x, transform.position.y + ladderHeight - topStartHeight, playerPosition.position.z), topEndPosition.position, playerPosition.localEulerAngles, topEndPosition.localEulerAngles, transform.position.y + ladderHeight - topStartHeight + 0.2f, transform.position.y + 0.1f, true);
        }
    }


}
