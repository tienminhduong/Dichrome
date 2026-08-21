using UnityEngine;


[DefaultExecutionOrder(0)]
public class WinGate : BaseSpecialNode
{
    [SerializeField] private CharacterColor requiredCharacterColor = CharacterColor.Black;
    private bool isActivated = false;

    protected override void Awake()
    {
        base.Awake();

        if (GameStateManager.HasInstance)
            GameStateManager.Instance.RegisterWinGate();
    }

    private void OnDestroy()
    {
        if (GameStateManager.HasInstance)
            GameStateManager.Instance.UnregisterWinGate();
    }

    public override void OnCharacterEnter(Character character)
    {
        if (character.CharacterColor != requiredCharacterColor)
            return;
        base.OnCharacterEnter(character);

        if (!isActivated)
        {
            isActivated = true;
            GameStateManager.Instance.ActivateWinGate();
        }
    }

    public override void OnCharacterExit(Character character)
    {
        if (character.CharacterColor != requiredCharacterColor)
            return;
        base.OnCharacterExit(character);

        if (isActivated)
        {
            isActivated = false;
            GameStateManager.Instance.DeactivateWinGate();
        }
    }
}
