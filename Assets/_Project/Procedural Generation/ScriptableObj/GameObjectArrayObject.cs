using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectArrayObject", menuName = "UnityObjects/GameObjectArrayObject", order = 40)]
public class GameObjectArrayObject : ScriptableObject
{
    public GameObject[] value;
}
