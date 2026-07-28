using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public GameObject Map { get; private set; }
    public string MapName { get; private set; }
    public BoxCollider Volume { get; private set; }

    Dictionary<Vector3, BaseObject> _objects = new Dictionary<Vector3, BaseObject>();

    public void LoadMap(string mapName)
    {
        DestroyMap();

        GameObject map = Managers.Resource.Instantiate(mapName);
        map.transform.position = Vector3.zero;
        map.name = $"@Map_{mapName}";

        Map = map;
        MapName = mapName;
        Volume = map.transform.Find("Volume").GetComponent<BoxCollider>();

        //SpawnObjectsByData(map, mapName);
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
