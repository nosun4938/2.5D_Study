using NUnit;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Define;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private List<BaseObject> _spawnObjects = new List<BaseObject>();
    public int StageIndex { get; set; }
    public StageVolume Volume { get; set; }
    public bool IsActive = false;

    public Vector3 _playerSpawnPoint;
    public Vector3 PlayerSpawnPoint
    {
        get { return  _playerSpawnPoint; }
        set { _playerSpawnPoint = value; }
    }
    public void SetInfo(int stageIndex)
    {
        StageIndex = stageIndex;
        Volume = Util.FindChild<StageVolume>(gameObject, "Volume", true);
        Volume.SetInfo(stageIndex);

        // Init
        IsActive = true;
        UnLoadStage();
    }

    public void LoadStage()
    {
        if (IsActive == true)
            return;

        IsActive = true;
        gameObject.SetActive(true);
        SpawnObjects();
    }
    public void UnLoadStage()
    {
        if (IsActive == false)
            return;

        IsActive = false;
        gameObject.SetActive(false);
        DespawnObjects();
    }

    private void SpawnObjects()
    {
        SpawnPoint[] points = this.GetComponentsInChildren<SpawnPoint>();

        foreach (var point in points)
        {
            int dataID = point.DataID;
            EObjectType objectType = point.ObjectType;

            switch (objectType)
            {
                case EObjectType.Hero:
                    PlayerSpawnPoint = point.transform.position;
                    break;
                case EObjectType.Monster:
                    Monster monster = Managers.Object.Spawn<Monster>(point.transform.position, dataID);
                    _spawnObjects.Add(monster);
                    break;
                case EObjectType.Npc:
                    Npc npc = Managers.Object.Spawn<Npc>(point.transform.position, dataID);
                    _spawnObjects.Add(npc);
                    break;
            }
        }
    }
    private void DespawnObjects()
    {
        foreach (BaseObject obj in _spawnObjects)
        {
            switch (obj.ObjectType)
            {
                case EObjectType.Monster:
                    Managers.Object.Despawn(obj as Monster);
                    break;
                case EObjectType.Npc:
                    Managers.Object.Despawn(obj as Npc);
                    break;
            }
        }

        _spawnObjects.Clear();
    }
}


