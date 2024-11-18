using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPosition : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private Vector3Object playerPosition;

    private void Update()
    {
        playerPosition.value = transform.position;
    }
}
