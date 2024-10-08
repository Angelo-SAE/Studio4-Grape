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
        Time.timeScale = 0;
        if(usingBoolObject)
        {
            gamePause.SetTrue();
        }
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        if(usingBoolObject)
        {
            gamePause.SetFalse();
        }
    }
}
