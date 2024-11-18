using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationNode
{
    public Vector2Int gridPosition;
    public Vector2 worldPosition;
    public int rotation;
    public int tileType;

    public GenerationNode(Vector2Int position, int size)
    {
        gridPosition = position;
        worldPosition = new Vector2(position.x * size, position.y * size);
    }
}
