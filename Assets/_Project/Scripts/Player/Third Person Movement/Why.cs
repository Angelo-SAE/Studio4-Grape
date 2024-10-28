using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Why : MonoBehaviour
{
    [SerializeField] private GameObject objectToFollow;

    private void Update()
    {
        transform.position = objectToFollow.transform.position;
    }
}
