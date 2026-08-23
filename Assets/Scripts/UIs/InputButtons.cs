using UnityEngine;
using UnityEngine.UI;


public class InputButtons : MonoBehaviour
{
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;

    [SerializeField] private Button flipButton;

    void OnEnable()
    {
        leftButton?.onClick.AddListener(LeftButtonPressed);
        rightButton?.onClick.AddListener(RightButtonPressed);
        flipButton?.onClick.AddListener(FlipButtonPressed);
        upButton?.onClick.AddListener(UpButtonPressed);
        downButton?.onClick.AddListener(DownButtonPressed);
    }

    void OnDisable()
    {
        leftButton?.onClick.RemoveListener(LeftButtonPressed);
        rightButton?.onClick.RemoveListener(RightButtonPressed);
        flipButton?.onClick.RemoveListener(FlipButtonPressed);
        upButton?.onClick.RemoveListener(UpButtonPressed);
        downButton?.onClick.RemoveListener(DownButtonPressed);
    }

    private void LeftButtonPressed()
    {
        if (InputHandler.IsInputLocked) return;
        InputHandler.Move?.Invoke(Vector2.left);
    }

    private void RightButtonPressed()
    {
        if (InputHandler.IsInputLocked) return;
        InputHandler.Move?.Invoke(Vector2.right);
    }

    private void UpButtonPressed()
    {
        if (InputHandler.IsInputLocked) return;
        InputHandler.Move?.Invoke(Vector2.up);
    }

    private void DownButtonPressed()
    {
        if (InputHandler.IsInputLocked) return;
        InputHandler.Move?.Invoke(Vector2.down);
    }

    private void FlipButtonPressed()
    {
        if (InputHandler.IsInputLocked) return;
        InputHandler.Swap?.Invoke();
    }
}
