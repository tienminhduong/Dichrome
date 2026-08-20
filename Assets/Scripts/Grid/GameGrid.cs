using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap wallTilemap;

    private Dictionary<Vector3Int, GridNode> gridNodes = new();
    [SerializeField] private List<GridNode> serializedNodes = new();

    void Awake()
    {
        LoadGridNodes();
        SerializeGridNodes();
    }

    private void LoadGridNodes()
    {
        gridNodes.Clear();

        foreach (var pos in walkableTilemap.cellBounds.allPositionsWithin)
        {
            if (!walkableTilemap.HasTile(pos))
                continue;
            gridNodes[pos] = new GridNode(pos, NodeType.Walkable);
            if (wallTilemap.HasTile(pos))
            {
                gridNodes[pos].nodeType = NodeType.Unwalkable;
            }
        }
    }

    private void SerializeGridNodes()
    {
        serializedNodes.Clear();
        foreach (var node in gridNodes.Values)
        {
            serializedNodes.Add(node);
        }
    }

    public GridNode GetGridNode(Vector3Int position)
    {
        if (gridNodes.TryGetValue(position, out var node))
        {
            return node;
        }
        return null;
    }

    public bool IsWalkable(Vector3Int position)
    {
        var node = GetGridNode(position);
        return node != null && (node.nodeType & NodeType.Walkable) != 0;
    }

    public Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 GridToWorldPosition(Vector3Int gridPosition)
    {
        return grid.GetCellCenterWorld(gridPosition);
    }
}