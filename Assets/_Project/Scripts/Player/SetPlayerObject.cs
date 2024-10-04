using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerObject : MonoBehaviour
{
    [Header("Scriptable Object")]
    [SerializeField] private GameObjectObject playerObject;
    [SerializeField] private BoolObject canInteract;

    private void Start()
    {
        playerObject.value = gameObject;
        canInteract.value = true;
    }

}
