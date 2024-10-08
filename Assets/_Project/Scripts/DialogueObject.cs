using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "ProjectObjects/DialogueObject", order = 0)]
public class DialogueObject : ScriptableObject
{
    [Header("NPC Variables")]
    public string npcName;
    public string[] npcDialogue;

    [Header("Player Variables")]
    public string playerName;
    public string[] playerDialogue;

    [Header("Dialogue Control Variables")] //When true it means the NPC is speaking when false it means the player is speaking
    public bool[] dialogueControl; //The size of the control array should be the combined size of both the player and npc dialogue
}
