using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap wallTilemap;

    private readonly Dictionary<Vector3Int, GridNode> gridNodes = new();
    private readonly Dictionary<Vector3Int, BaseSpecialNode> specialInitNodes = new();
    [SerializeField] private List<GridNode> serializedNodes = new();

    void Awake()
    {
        LoadGridNodes();
        SerializeGridNodes();
    }

    private void LoadGridNodes()
    {
        LogService.Log($"Loading grid nodes for {gameObject.name}..., current size of gridNodes: {gridNodes.Count}");
        foreach (var pos in walkableTilemap.cellBounds.allPositionsWithin)
        {
            if (!walkableTilemap.HasTile(pos))
                continue;
            if (!gridNodes.ContainsKey(pos))
                gridNodes[pos] = new GridNode(pos, wallTilemap.HasTile(pos) ? NodeType.Unwalkable : NodeType.Walkable);
            else if (wallTilemap.HasTile(pos))
                gridNodes[pos].nodeType |= NodeType.Unwalkable;
            else
                gridNodes[pos].nodeType |= NodeType.Walkable;
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

    public void AddSpecialGridNode(BaseSpecialNode node)
    {
        var gridPosition = WorldToGridPosition(node.transform.position);
        if (!gridNodes.ContainsKey(gridPosition))
        {
            var gridNode = new GridNode(gridPosition, NodeType.Effect);
            gridNodes[gridPosition] = gridNode;
        }
        else
        {
            gridNodes[gridPosition].nodeType |= NodeType.Effect;
        }
        specialInitNodes[gridPosition] = node;
    }

    public void RemoveSpecialGridNode(BaseSpecialNode node)
    {
        var gridPosition = WorldToGridPosition(node.transform.position);
        if (gridNodes.ContainsKey(gridPosition))
        {
            gridNodes[gridPosition].nodeType &= ~NodeType.Effect;
            specialInitNodes.Remove(gridPosition);
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

    public void HandleSpecialEffectAtPosition(Vector3Int position, Character character)
    {
        var node = GetGridNode(position);
        if (node != null && (node.nodeType & NodeType.Effect) != 0)
        {
            if (specialInitNodes.TryGetValue(position, out var specialNode))
            {
                specialNode.OnCharacterEnter(character);
            }
        }
    }
}