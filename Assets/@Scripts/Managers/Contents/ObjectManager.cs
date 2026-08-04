using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public Player Player { get; private set; }
    public HashSet<Npc> Npcs { get; } = new HashSet<Npc>();


    #region Roots
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }
    
    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    public Transform BossRoot { get { return GetRootTransform("@Bosses"); } }
    public Transform ArtifactRoot { get { return GetRootTransform("@Artifacts"); } }
    public Transform ItemRoot { get { return GetRootTransform("@Items"); } }
    public Transform InteractionRoot { get { return GetRootTransform("@Interactions"); } }
    public Transform NpcRoot { get { return GetRootTransform("@Npcs"); } }
    #endregion

    public T Spawn<T>(Vector3 position, int templateID) where T : BaseObject
    {
        string prefabName = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(prefabName);
        go.name = prefabName;
        go.transform.position = position;

        BaseObject obj = go.GetComponent<BaseObject>();
        
        if (obj.ObjectType == EObjectType.Player)
        {
            Debug.Log("Player Spawn");
            Player player = go.GetComponent<Player>();
            Player = player;
            player.SetInfo(templateID);
        }
        else if (obj.ObjectType == EObjectType.Npc)
        {
            Debug.Log("NPC Spawn");
            obj.transform.parent = NpcRoot;
            Npc npc = go.GetComponent<Npc>();
            Npcs.Add(npc);
            npc.SetInfo(templateID);
        }

        return obj as T;
    }

    public void Despawn<T>(T obj) where T : BaseObject
    {
        if (obj == null)
            return;

        EObjectType objectType = obj.ObjectType;

        if (objectType == EObjectType.Player)
        {
            Player = null;
        }
        else if (objectType == EObjectType.Npc)
        {
            Npc npc = obj as Npc;
            Npcs.Remove(npc);
        }

        Managers.Resource.Destroy(obj.gameObject);
        Debug.Log($"{obj} is Despawned");
    }
}
