using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public Player Player { get; private set; }
    public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();
    public HashSet<Npc> Npcs { get; } = new HashSet<Npc>();
    public HashSet<EffectBase> Effects { get; } = new HashSet<EffectBase>();
    public HashSet<ItemHolder> ItemHolders { get; } = new HashSet<ItemHolder>();


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
    public Transform InteractionRoot { get { return GetRootTransform("@Interactions"); } }
    public Transform NpcRoot { get { return GetRootTransform("@Npcs"); } }
    public Transform EffectRoot { get { return GetRootTransform("@Effects"); } }
    public Transform ItemHolderRoot { get { return GetRootTransform("@ItemHolders"); } }

    #endregion

    public void ShowDamageFont(Vector3 position, float damage, Transform parent, bool isCritical = false)
    {
        GameObject go = Managers.Resource.Instantiate("DamageFont", pooling: true);
        DamageFont damageText = go.GetComponent<DamageFont>();
        damageText.SetInfo(position, damage, parent, isCritical);
    }
    public GameObject SpawnGameObject(Vector3 position, string prefabName)
    {
        GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
        go.transform.position = position;
        return go;
    }

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
        else if (obj.ObjectType == EObjectType.Monster)
        {
            Debug.Log("Monster Spawn");
            obj.transform.parent = MonsterRoot;
            Monster monster = go.GetComponent<Monster>();
            Monsters.Add(monster);
            monster.SetInfo(templateID);
        }
        else if (obj.ObjectType == EObjectType.Npc)
        {
            Debug.Log("NPC Spawn");
            obj.transform.parent = NpcRoot;
            Npc npc = go.GetComponent<Npc>();
            Npcs.Add(npc);
            npc.SetInfo(templateID);
        }
        else if (obj.ObjectType == EObjectType.ItemHolder)
        {
            Debug.Log("ItemHolder Spawn");
            obj.transform.parent = ItemHolderRoot;
            ItemHolder itemHolder = go.GetComponent<ItemHolder>();
            ItemHolders.Add(itemHolder);
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
        else if (objectType == EObjectType.Monster)
        {
            Monster monster = obj as Monster;
            Monsters.Remove(monster);
        }
        else if (objectType == EObjectType.Npc)
        {
            Npc npc = obj as Npc;
            Npcs.Remove(npc);
        }
        else if (objectType == EObjectType.Effect)
        {
            EffectBase effect = obj as EffectBase;
            Effects.Remove(effect);
        }
        else if (objectType == EObjectType.ItemHolder)
        {
            ItemHolder itemHolder = obj as ItemHolder;
            ItemHolders.Remove(itemHolder);
        }

        Managers.Resource.Destroy(obj.gameObject);
        Debug.Log($"{obj} is Despawned");
    }
}
