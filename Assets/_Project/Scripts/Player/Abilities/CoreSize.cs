using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSize : MonoBehaviour
{

    public GameObject cylinder;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    public void UpdateSize(float radius)
    {
        Vector3 newScale = cylinder.transform.localScale;
        newScale.x = 2*radius;
        newScale.z = 2*radius;

        cylinder.transform.localScale = newScale;

    }
}
