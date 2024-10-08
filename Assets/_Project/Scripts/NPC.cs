using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("ScriptableObjects")]
    [SerializeField] private GameObjectObject dialogueBox;
    [SerializeField] private IntObject npcDialogueCount;
    [SerializeField] private DialogueObject[] npcDialogue;

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
            dialogueBox.value.GetComponent<DialogueBox>().StartDialogue(npcDialogue[npcDialogueCount.value]);
            npcDialogueCount.value++;
            isInteractable = false;
        }
    }

    public void AltInteract() {}
}
