using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnKeyEvent : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private BoolObject paused;

    [Header("Key Code")]
    [SerializeField] private KeyCode keyCode;

    [Header("Events")]
    [SerializeField] private UnityEvent FirstEvent;
    [SerializeField] private UnityEvent SecondEvent;

    private bool isActive;

    private void Update()
    {
        if(Input.GetKeyDown(keyCode) && !isActive && !paused.value)
        {
            isActive = true;
            FirstEvent.Invoke();
        } else if(Input.GetKeyDown(keyCode) && isActive)
        {
            isActive = false;
            SecondEvent.Invoke();
        }
    }
}
