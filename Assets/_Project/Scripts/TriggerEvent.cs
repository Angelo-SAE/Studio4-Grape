using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private UnityEvent onTriggerEvent;

    [Header("Trigger Variables")]
    [SerializeField] private string triggerTag;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag is not null)
        {
            if(col.tag == triggerTag)
            {
                onTriggerEvent.Invoke();
            }
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.tag is not null)
        {
            if (col.tag == triggerTag)
            {
                onTriggerEvent.Invoke();
            }
        }
    }
}
