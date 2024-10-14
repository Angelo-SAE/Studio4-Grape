using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KeyBindingsObject : ScriptableObject
{
    [Header("Key Bindings Array")]
    public KeyCode[] keyArray;

    [Header("Player Movement Keys")]
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode right;
    public KeyCode left;
    public KeyCode sprint;
    public KeyCode crouch;
    public KeyCode jump;

    [Header("Player Combat Keys")]
    public KeyCode primaryFire;
    public KeyCode abilityOne;
    public KeyCode abilityTwo;
    public KeyCode abilityThree;

    [Header("Extra Player Keys")]
    public KeyCode interact;
    public KeyCode altInteract;

    private void OnValidate()
    {
        keyArray = new KeyCode[13];
        keyArray[0] = forward;
        keyArray[1] = backward;
        keyArray[2] = right;
        keyArray[3] = left;
        keyArray[4] = sprint;
        keyArray[5] = crouch;
        keyArray[6] = jump;
        keyArray[7] = primaryFire;
        keyArray[8] = abilityOne;
        keyArray[9] = abilityTwo;
        keyArray[10] = abilityThree;
        keyArray[11] = interact;
        keyArray[12] = altInteract;

    }

    public void UpdateKeyCodes()
    {
        forward = keyArray[0];
        backward = keyArray[1];
        right = keyArray[2];
        left = keyArray[3];
        sprint = keyArray[4];
        crouch = keyArray[5];
        jump = keyArray[6];
        primaryFire = keyArray[7];
        abilityOne = keyArray[8];
        abilityTwo = keyArray[9];
        abilityThree = keyArray[10];
        interact = keyArray[11];
        altInteract = keyArray[12];
    }
}
