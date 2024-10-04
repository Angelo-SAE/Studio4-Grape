using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolObject", menuName = "VariableObjects/BoolObject", order = 0)]
public class BoolObject : ScriptableObject
{
    public bool value;

    public void SetTrue()
    {
      value = true;
    }

    public void SetFalse()
    {
      value = false;
    }
}
