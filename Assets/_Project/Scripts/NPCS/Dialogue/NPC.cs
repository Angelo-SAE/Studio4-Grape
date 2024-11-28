using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("ScriptableObjects")]
    [SerializeField] private GameObjectObject dialogueBox;
    [SerializeField] private IntObject npcDialogueCount;
    [SerializeField] private DialogueObject[] npcDialogue;
    [SerializeField] private GameObjectObject playerObject;

    [Header("NPC Variables")]
    [SerializeField] private bool lookAtPlayerWhenTalking;

    [Header("Events")]
    [SerializeField] private UnityEvent OnInteract;
    [SerializeField] private UnityEvent AfterDialogue;

    [Header("NPC Animator")]
    [SerializeField] private Animator animator;

    private bool isInteractable;

    private void Start() //For testing
    {
        npcDialogueCount.value = 0;
        isInteractable = true;
    }

    public bool CheckIfInteractable()
    {
        return isInteractable;
    }

    public void EnableNPC()
    {
        if(npcDialogueCount.value < npcDialogue.Length)
        {
            isInteractable = true;
        }
    }

    public void Interact()
    {
        if(npcDialogueCount.value < npcDialogue.Length)
        {
            OnInteract.Invoke();
            if(lookAtPlayerWhenTalking)
            {
                LookAtPlayer();
            }
            dialogueBox.value.GetComponent<DialogueBox>().StartDialogue(this, npcDialogue[npcDialogueCount.value]);
            npcDialogueCount.value++;
            isInteractable = false;
        }
    }

    public void AltInteract() {}

    private void LookAtPlayer()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Vector3.Angle(Vector3.forward, playerObject.value.transform.position - transform.position), transform.eulerAngles.z);
        animator.Play("Talking");
    }

    public void EndDialogue()
    {
        AfterDialogue.Invoke();
    }
}
