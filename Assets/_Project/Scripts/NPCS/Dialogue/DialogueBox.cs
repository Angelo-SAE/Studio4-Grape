using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueBox : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject dialogueBox;

    [Header("DialogueBox Variables")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float dialogueSpeed;

    private bool isTalking;
    private bool isWaiting;
    private int npcDialogueCount;
    private int playerDialogueCount;
    private DialogueObject dialogue;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent OnDialogueStart;
    [SerializeField] private UnityEvent OnDialogueEnd;

    private void Start()
    {
        dialogueBox.value = gameObject;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if(isTalking)
            {
                isTalking = false;
            } else if(isWaiting)
            {
                isWaiting = false;
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(DialogueObject milkyDial)
    {
        dialogue = milkyDial;
        dialogueText.text = "";
        npcDialogueCount = 0;
        playerDialogueCount = 0;

        OnDialogueStart.Invoke();
        if(dialogue.dialogueControl[npcDialogueCount + playerDialogueCount])
        {
            nameText.text = dialogue.npcName;
            StartCoroutine(DisplayText(dialogue.npcDialogue[npcDialogueCount]));
            npcDialogueCount++;
        } else {
            nameText.text = dialogue.playerName;
            StartCoroutine(DisplayText(dialogue.playerDialogue[playerDialogueCount]));
            playerDialogueCount++;
        }

    }

    private void DisplayNextLine()
    {
        if((npcDialogueCount + playerDialogueCount) < dialogue.dialogueControl.Length)
        {
            dialogueText.text = "";
            if(dialogue.dialogueControl[npcDialogueCount + playerDialogueCount])
            {
                nameText.text = dialogue.npcName;
                StartCoroutine(DisplayText(dialogue.npcDialogue[npcDialogueCount]));
                npcDialogueCount++;
            } else {
                nameText.text = dialogue.playerName;
                StartCoroutine(DisplayText(dialogue.playerDialogue[playerDialogueCount]));
                playerDialogueCount++;
            }
        } else {
            OnDialogueEnd.Invoke();
        }

    }

    private IEnumerator DisplayText(string text)
    {
        isTalking = true;
        for(int a = 0; a < text.Length; a++)
        {
            if(isTalking)
            {
                dialogueText.text += text[a];
                yield return new WaitForSeconds(dialogueSpeed * 0.01f);
            } else {
                dialogueText.text = text;
                break;
            }
        }
        yield return new WaitForSeconds(0.3f);
        isTalking = false;
        isWaiting = true;
    }
}
