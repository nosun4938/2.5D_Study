using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using static Define;

public class LoadScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.LoadScene;

        return true;
    }

    public override void Clear()
    {

    }
}
