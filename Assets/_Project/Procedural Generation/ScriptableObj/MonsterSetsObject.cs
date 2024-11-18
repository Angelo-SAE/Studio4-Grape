using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSetsObject", menuName = "MonsterSetsObject", order = 0)]
public class MonsterSetsObject : ScriptableObject
{
    public int currentMaxSpawn;
    public GameObject[] sets;
    public int[] spawnCosts;

}
