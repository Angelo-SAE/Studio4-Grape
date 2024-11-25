using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectObject playerObject;

    [Header("Teleport Positions")]
    [SerializeField] private Vector3 firstPosition;
    [SerializeField] private Vector3 secondPosition;

    public void EnterSafeZone()
    {
        playerObject.value.transform.position = secondPosition;
    }

    public void ExitSafeZone()
    {
        playerObject.value.transform.position = firstPosition;
    }
}
