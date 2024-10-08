using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ConfineCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
    }

    public void RevealCursor()
    {
        Cursor.visible = true;
    }
}
