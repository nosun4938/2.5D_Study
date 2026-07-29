using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Define;
using static Util;

public class GameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = EScene.GameScene;

        // Map
        Managers.Map.LoadMap("TestMap");
        Managers.Map.StageTransition.SetInfo();

        // Hero
        Player player = Managers.Object.Spawn<Player>(new Vector3(0, 0, 0), 202001);

        // Camera
        CameraController camera = Camera.main.GetComponent<CameraController>();
        camera.SetInfo(player);
        camera.SetMap();
        // UI

        // Sound

        return true;
    }

    public override void Clear()
    {
        
    }
}
