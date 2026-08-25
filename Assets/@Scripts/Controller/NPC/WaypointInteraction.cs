using UnityEngine;

public class WaypointInteraction : INpcInteraction
{
    private Npc _owner;
    public void SetInfo(Npc owner)
    {
        _owner = owner;
    }

    public bool CanInteract()
    {
        return true;
    }

    public void HandleOnClickEvent()
    {
        Managers.UI.ShowPopupUI<UI_WaypointPopup>();
    }
}
