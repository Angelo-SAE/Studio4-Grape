using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    [Header("Pause Value")] //find better name
    [SerializeField] private bool usingBoolObject;
    [SerializeField] private BoolObject gamePause;

    private void Start()
    {
        UnPauseGame();
    }

    public void PauseGame()
    {
        if(usingBoolObject)
        {
            gamePause.SetTrue();
        }
    }

    public void UnPauseGame()
    {
        if(usingBoolObject)
        {
            gamePause.SetFalse();
        }
    }
}
