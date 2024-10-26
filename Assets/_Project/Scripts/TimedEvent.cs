using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvent : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float timeTillEvent;
    [SerializeField] private UnityEvent timedEvent;

    private float currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= timeTillEvent)
        {
            timedEvent.Invoke();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
