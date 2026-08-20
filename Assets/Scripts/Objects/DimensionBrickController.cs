using System.Collections.Generic;
using UnityEngine;

public class DimensionBrickController : Singleton<DimensionBrickController>
{
    [SerializeField] private List<Sprite> stateSprites = new();
    [SerializeField] private List<Material> stateMaterials = new();

    public Material GetDifferentMaterial(Material currentMaterial)
    {
        LogService.Log($"Current Material: {currentMaterial.name}, State Materials: {stateMaterials[0].name}, {stateMaterials[1].name}");
        return currentMaterial.name == stateMaterials[0].name ? stateMaterials[1] : stateMaterials[0];
    }

    public Sprite GetSpriteForState(BrickState state)
    {
        return stateSprites[(int)state];
    }
}

public enum BrickState
{
    Walkable,
    Unwalkable
}