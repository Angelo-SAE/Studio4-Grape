using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatArrayObject", menuName = "VariableArrayObjects/FloatArrayObject", order = 21)]
public class FloatArrayObject : ScriptableObject
{
    public float[] value;
}
