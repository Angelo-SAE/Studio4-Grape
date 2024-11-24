using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToRandom : MonoBehaviour
{
    private void Start()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
    }
}
