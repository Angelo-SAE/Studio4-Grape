using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringArrayObject", menuName = "VariableArrayObjects/StringArrayObject", order = 23)]
public class StringArrayObject : ScriptableObject
{
    public string[] value;
}
