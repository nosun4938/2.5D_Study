using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ObjectManager
{
    //public HashSet<Hero> Heroes { get; } = new HashSet<Hero>();
    //public Player Player { get; private set; }


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
    #endregion

    public T Spawn<T>(Vector3 position, int templateID) where T : BaseObject
    {
        string prefabName = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(prefabName);
        go.name = prefabName;
        go.transform.position = position;

        BaseObject obj = go.GetComponent<BaseObject>();
        
        if (obj.ObjectType == EObjectType.Creature)
        {
            Creature creature = go.GetComponent<Creature>();
            switch (creature.CreatureType)
            {
                case ECreatureType.Player:
                    Debug.Log("Hero Spawn");
                    //Hero hero = creature as Hero;
                    //Player = hero;
                    //hero.InitVector = position;
                    break;
            }

            creature.SetInfo(templateID);
        }

        return obj as T;
    }

    public void Despawn<T>(T obj) where T : BaseObject
    {
        if (obj == null)
            return;

        EObjectType objectType = obj.ObjectType;

        if (obj.ObjectType == EObjectType.Creature)
        {
            Creature creature = obj.GetComponent<Creature>();
            switch (creature.CreatureType)
            {
                case ECreatureType.Player:
                    //Hero hero = creature as Hero;
                    //Player = null;
                    break;
            }
        }

        Managers.Resource.Destroy(obj.gameObject);
        Debug.Log($"{obj} is Despawned");
    }
}
