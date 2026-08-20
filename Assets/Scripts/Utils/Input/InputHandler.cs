using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

public class InputHandler : MonoBehaviour, IPlayerActions, IUIActions
{
    public static event Action<Vector2> Move;
    public static event Action Interact;

    InputSystem_Actions inputActions;

    public void EnablePlayerActions()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();

            inputActions.Player.SetCallbacks(this);
            inputActions.UI.SetCallbacks(this);
        }

        inputActions.Enable();
        inputActions.UI.Disable();
    }

    void OnEnable()
    {
        EnablePlayerActions();
        PublicEvents.OnUIOpened += FocusUI;
        PublicEvents.OnUIClosed += FocusPlayer;
    }

    void OnDisable()
    {
        inputActions.Disable();
        PublicEvents.OnUIOpened -= FocusUI;
        PublicEvents.OnUIClosed -= FocusPlayer;
    }

    void FocusUI()
    {
        inputActions.Disable();
        inputActions.UI.Enable();
    }

    void FocusPlayer()
    {
        inputActions.UI.Disable();
        inputActions.Enable();
    }
    public void OnCancel(InputAction.CallbackContext context) { }

    public void OnClick(InputAction.CallbackContext context) { }

    public void OnMiddleClick(InputAction.CallbackContext context) { }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Move?.Invoke(input);
        }
    }

    public void OnNavigate(InputAction.CallbackContext context) { }

    public void OnPoint(InputAction.CallbackContext context) { }

    public void OnRightClick(InputAction.CallbackContext context) { }

    public void OnScrollWheel(InputAction.CallbackContext context) { }

    public void OnSubmit(InputAction.CallbackContext context) { }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
}
