using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MapManager
{
    public Stage CurrentStage { get; set; }
    public GameObject Map { get; private set; }
    public string MapName { get; private set; }
    public BoxCollider Volume { get; private set; }

    Dictionary<Vector3, BaseObject> _objects = new Dictionary<Vector3, BaseObject>();
    public StageTransition StageTransition;

    public void LoadMap(string mapName)
    {
        DestroyMap();

        GameObject map = Managers.Resource.Instantiate(mapName);
        map.transform.position = Vector3.zero;
        map.name = $"@Map_{mapName}";

        StageTransition = map.GetComponent<StageTransition>();

        Map = map;
        MapName = mapName;
        //Volume = map.transform.Find("Volume").GetComponent<BoxCollider>();
    }

    public void DestroyMap()
    {
        ClearObjects();

        if (Map != null)
            Managers.Resource.Destroy(Map);
    }

    public void ClearObjects()
    {
        _objects.Clear();
    }
}
