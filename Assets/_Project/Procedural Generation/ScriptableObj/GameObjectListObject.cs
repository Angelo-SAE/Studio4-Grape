using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectListObject", menuName = "UnityObjects/GameObjectListObject", order = 41)]
public class GameObjectListObject : ScriptableObject
{
    public List<GameObject> value;
}
