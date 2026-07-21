using Unity.Cinemachine;
using UnityEngine;

public class CameraController : InitBase
{
    [SerializeField]
    CinemachineCamera _cinemachineCamera;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(Player player)
    {
        _cinemachineCamera.Target.TrackingTarget = player.transform;
    }
}
