using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolArrayObject", menuName = "VariableArrayObjects/BoolArrayObject", order = 20)]
public class BoolArrayObject : ScriptableObject
{
    public bool[] value;
}
