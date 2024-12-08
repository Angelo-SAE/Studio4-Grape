using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MissionExit : MonoBehaviour
{
    [SerializeField] private UnityEvent exitEvent;

    public void Exit()
    {
        Debug.Log("exited Mission");
        exitEvent.Invoke();
    }
}
