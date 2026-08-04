using UnityEngine;

public class UI_NpcInteraction : UI_Base
{
    private Npc _owner;

    enum Images
    {
        InteractionIcon
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImages(typeof(Images));
        GetComponent<Canvas>().worldCamera = Camera.main;

        return true;
    }

    public void SetInfo(int dataId, Npc owner)
    {
        _owner = owner;
    }
}
