using System;
using UnityEngine;

[Serializable]
public class GridNode
{
    public Vector3Int position;
    public NodeType nodeType;

    public GridNode(Vector3Int pos, NodeType type)
    {
        position = pos;
        nodeType = type;
    }
}

public enum NodeType
{
    Walkable = 1 << 1,
    Unwalkable = 1 << 2,
    Effect = 1 << 3,
}