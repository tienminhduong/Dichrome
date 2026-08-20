using DG.Tweening;
using UnityEngine;

public class TeleportGate : BaseSpecialNode
{
    [SerializeField] private TeleportGate linkedGate;

    private bool isEnabled = true;
    private Vector3 normalScale;

    public override void OnCharacterEnter(Character character)
    {
        LogService.Log($"Character {character.gameObject.name} entered TeleportGate {gameObject.name}");
        if (linkedGate == null)
        {
            LogService.LogError($"TeleportGate {gameObject.name} does not have a linked gate. Please assign a linked gate in the inspector.");
            return;
        }
        if (!isEnabled)
        {
            LogService.Log($"TeleportGate {gameObject.name} is disabled. No teleportation will occur.");
            return;
        }

        character.PlayScaleDownTween().OnComplete(() =>
        {
            character.ChangeGrid(linkedGate.gameGrid);

            linkedGate.isEnabled = false;
            isEnabled = false;

            character.transform.position = linkedGate.transform.position;
            character.PlayScaleUpTween();

            PlayCloseGateAnimation();
            linkedGate.PlayCloseGateAnimation();
        });
    }

    private Tween PlayOpenGateAnimation()
    {
        return transform.DOScaleY(normalScale.y, CommonSetting.Instance.MoveAnimTime)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                EnableNode();
            });
    }

    private Tween PlayCloseGateAnimation()
    {
        normalScale = transform.localScale;
        return transform.DOScaleY(0, CommonSetting.Instance.MoveAnimTime)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                DisableNode();
            });
    }
}
