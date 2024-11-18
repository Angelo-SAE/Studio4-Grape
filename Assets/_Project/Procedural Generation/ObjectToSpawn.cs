using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToSpawn : MonoBehaviour
{
    [Header("Object Variables")]
    [SerializeField] private Vector2Int size;
    [SerializeField] private bool rotateObject;

    public Vector2Int Size => size;
    public bool RotateObject => rotateObject;
}
