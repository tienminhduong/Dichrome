using UnityEngine;

public class TeleportGate : BaseSpecialNode
{
    [SerializeField] private TeleportGate linkedGate;

    private bool isEnabled = true;

    public override void OnCharacterEnter(Character character)
    {
        LogService.Log($"Character {character.gameObject.name} entered TeleportGate {gameObject.name}");
    }
}
