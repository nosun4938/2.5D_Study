using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    private InputActionAsset _actions;

    #region UI Actions
    private InputAction _cancelAction;
    private InputAction _submitAction;
    #endregion

    public void Init()
    {
        _actions = Managers.Resource.Load<InputActionAsset>("InputSystem_Actions");
        _actions.Enable();

        _cancelAction = _actions.FindActionMap("UI").FindAction("Cancel");
        _cancelAction.performed += OnCancel;

        _submitAction = _actions.FindActionMap("UI").FindAction("Submit");
        _submitAction.performed += OnSubmit;
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (Managers.UI.ClosePopupUI() == false)
        {
            //Managers.UI.ShowPopupUI<UI_Settings>();
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {

    }

    public void Clear()
    {
        _cancelAction.performed -= OnCancel;
        _actions.Disable();
    }

    #region Player Input
    public void PlayerInputLock()
    {
        Hero hero = Managers.Object.Player;
        if (hero != null)
        {
            hero.PlayerInputLock();
        }
    }

    public void PlayerInputUnlock()
    {
        Hero hero = Managers.Object.Player;
        if (hero != null)
        {
            hero.PlayerInputUnlock();
        }
    }
    #endregion
}
