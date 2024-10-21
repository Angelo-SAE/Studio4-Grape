using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Directions
{
    public static List<Vector3> directions = new List<Vector3>
    {
        new Vector3(0,0,1),
        new Vector3(0.5f,0,0.5f),
        new Vector3(1,0,0),
        new Vector3(0.5f,0,-0.5f),
        new Vector3(0,0,-1),
        new Vector3(-0.5f,0,-0.5f),
        new Vector3(-1,0,0),
        new Vector3(-0.5f,0,0.5f)
    };
}
