using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSize : MonoBehaviour
{

    public GameObject cylinder;
    
    
    public void UpdateSize(float radius)
    {
        Vector3 newScale = cylinder.transform.localScale;
        newScale.x = 2*radius;
        newScale.z = 2*radius;

        cylinder.transform.localScale = newScale;

    }
}
