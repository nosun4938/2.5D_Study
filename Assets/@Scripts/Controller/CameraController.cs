using Unity.Cinemachine;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class CameraController : InitBase
{
    [SerializeField]
    CinemachineCamera _cinemachineCamera;
    CinemachinePositionComposer _cinemachinePosition;
    CinemachineConfiner3D _cinemachineConfiner;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        GameObject go = GameObject.Find("@CinemachineCamera");
        if (go == null)
        {
            go = new GameObject { name = "@CinemachineCamera" };
            go.AddComponent<CinemachineCamera>();
            go.AddComponent<CinemachinePositionComposer>();
            go.AddComponent<CinemachineConfiner3D>();
        }

        _cinemachineCamera = go.GetComponent<CinemachineCamera>();
        _cinemachinePosition = go.GetComponent<CinemachinePositionComposer>();
        _cinemachineConfiner = go.GetComponent <CinemachineConfiner3D>();
        return true;
    }

    public void SetInfo(Player player)
    {
        _cinemachineCamera.Target.TrackingTarget = player.transform;
        _cinemachineCamera.Lens.FieldOfView = 60;

        _cinemachinePosition.CameraDistance = 50;
        _cinemachinePosition.Composition.ScreenPosition = new Vector2(0, 0.2f);
        _cinemachinePosition.Composition.DeadZone.Enabled = true;
        _cinemachinePosition.Composition.DeadZone.Size = new Vector2(0.125f, 0.04f);
        _cinemachinePosition.Damping = new Vector3(1, 1, 1);
    }

    public void SetMap(Collider collider)
    {
        _cinemachineConfiner.BoundingVolume = collider;
    }
}
