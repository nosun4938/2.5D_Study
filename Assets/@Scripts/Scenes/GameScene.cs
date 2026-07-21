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

        // Hero, 가장 먼저 생성해야함 
        Player player = Managers.Object.Spawn<Player>(new Vector3Int(0, 0, 0), 202001);

        // Map

        // Camera
        CameraController camera = Camera.main.GetComponent<CameraController>();
        camera.SetInfo(player);
        // UI

        // Sound

        return true;
    }

    public override void Clear()
    {
        
    }
}
