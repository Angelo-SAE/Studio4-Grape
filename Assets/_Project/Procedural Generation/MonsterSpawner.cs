using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private GameObjectListObject spawnerList;

    private void Start()
    {
        spawnerList.value.Add(gameObject);
    }
}
