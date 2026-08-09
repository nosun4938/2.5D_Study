using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_NpcInteraction : UI_Base
{
    private Npc _owner;

    enum Buttons
    {
        InteractionButton
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        GetComponent<Canvas>().worldCamera = Camera.main;

        return true;
    }

    public void SetInfo(int dataId, Npc owner)
    {
        _owner = owner;
        GetButton((int)Buttons.InteractionButton).gameObject.BindEvent(OnClickInteractionButton);
    }

    private void OnClickInteractionButton(PointerEventData evt)
    {
        Debug.Log("On Click Button");
        switch (_owner.NpcData.NpcType)
        {
            case ENpcType.Waypoint:
                Managers.UI.ShowPopupUI<UI_WaypointPopup>();
                break;
            default:
                break;
        }
    }
}
