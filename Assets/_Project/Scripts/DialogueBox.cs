using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueBox : MonoBehaviour
{
    [Header("Temp Variables")]
    [SerializeField] private StringArrayObject lin;
    [SerializeField] private string nam;

    [Header("DialogueBox Variables")]
    [SerializeField] private TMP_Text npcName;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float dialogueSpeed;

    private string[] lines;
    private string name;
    private bool isTalking;
    private bool isWaiting;
    private int currentLine;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent OnDialogueStart;
    [SerializeField] private UnityEvent OnDialogueEnd;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartDialogue(nam, lin.value);
        }
        //if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && isTalking)
        //{
        //    isTalking = false;
        //}
        //if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && isWaiting)
        //{
        //    isWaiting = false;
        //    DisplayNextLine();
        //}
    }

    private void StartDialogue(string nae, string[] lies)
    {
        name = nae;
        lines = lies;
        npcName.text = name;
        dialogueText.text = "";
        currentLine = 0;

        OnDialogueStart.Invoke();

        StartCoroutine(DisplayText(lines[currentLine]));
    }

    private void DisplayNextLine()
    {
        StartCoroutine(DisplayText(lines[currentLine]));
    }

    private IEnumerator DisplayText(string text)
    {
        isTalking = true;
        for(int a = 0; a < text.Length; a++)
        {
            dialogueText.text += text[a];
            yield return new WaitForSeconds(dialogueSpeed * 0.01f);
        }
    }
}
