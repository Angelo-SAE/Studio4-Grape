using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntArrayObject", menuName = "VariableArrayObjects/IntArrayObject", order = 22)]
public class IntArrayObject : ScriptableObject
{
    public int[] value;
}
