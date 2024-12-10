using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;

    [Header("Teleport Positions")]
    [SerializeField] private Transform firstPosition;
    [SerializeField] private Transform secondPosition;

    public void EnterSafeZone()
    {
        playerObject.value.transform.position = secondPosition.position;
    }

    public void ExitSafeZone()
    {
        playerObject.value.transform.position = firstPosition.position;
    }
}
