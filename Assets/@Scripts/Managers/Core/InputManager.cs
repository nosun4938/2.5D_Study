using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    private InputActionAsset _actions;

    #region UI Actions
    private InputAction _cancelAction;
    #endregion

    public void Init()
    {
        _actions = Managers.Resource.Load<InputActionAsset>("InputSystem_Actions");
        _actions.Enable();

        _cancelAction = _actions.FindActionMap("UI").FindAction("Cancel");
        _cancelAction.performed += OnCancel;
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (Managers.UI.ClosePopupUI() == false)
        {
            //Managers.UI.ShowPopupUI<UI_Settings>();
        }
    }

    public void Clear()
    {
        _cancelAction.performed -= OnCancel;
        _actions.Disable();
    }
}
