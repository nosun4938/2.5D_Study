using Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using static Define;

[Serializable]
public class GameSaveData
{
    public int Money = 0;
    public int PlayerLevel = 1;

    public int ItemDbIDGenerator = 1;
    public List<HeroSaveData> Heroes = new List<HeroSaveData>();
    public List<ItemSaveData> Items = new List<ItemSaveData>();
    public List<QuestSaveData> AllQuests = new List<QuestSaveData>();
}

[Serializable]
public class HeroSaveData
{
    public int Level = 1;
    public int Exp = 10;

    public int DataID = 0;
    public EHeroOwningState OwningState = EHeroOwningState.Unowned;
}

[Serializable]
public class ItemSaveData
{
    public int InstanceID;
    public int DbID;

    public int TemplateID;
    public int Count;
    public int EquipSlot;
}

[Serializable]
public class QuestSaveData
{
    public int TemplateID;
    public EQuestState State = EQuestState.None;
    public List<int> ProgressCount = new List<int>();
    public DateTime NextResetTime;
}

public class GameManager
{
    #region GameData
    GameSaveData _saveData = new GameSaveData();
    public GameSaveData SaveData { get { return _saveData; } set { _saveData = value; } }

    public int Money
    {
        get { return _saveData.Money; }
        private set
        {
            int diff = _saveData.Money - value;
            _saveData.Money = value;
            OnBroadcastEvent?.Invoke(EBroadcastEventType.ChangeMoney, diff);
        }
    }

    public bool CheckResource(EResourceType eResourceType, int amount)
    {
        switch (eResourceType)
        {
            case EResourceType.Money:
                return Money >= amount;
            default:
                return false;
        }
    }

    public bool SpendResource(EResourceType eResourceType, int amount)
    {
        if (CheckResource(eResourceType, amount) == false)
            return false;

        switch (eResourceType)
        {
            case EResourceType.Money:
                Money -= amount;
                break;
        }
        return true;
    }

    public void EarnResource(EResourceType eResourceType, int amount)
    {
        switch (eResourceType)
        {
            case EResourceType.Money:
                Money += amount;
                break;
        }
    }

    public void BroadcastEvent(EBroadcastEventType eventType, int value)
    {
        OnBroadcastEvent?.Invoke(eventType, value);
    }

    public List<HeroSaveData> AllHeroes { get { return _saveData.Heroes; } }
    public int TotalHeroCount { get { return _saveData.Heroes.Count; } }
    public int UnownedHeroCount { get { return _saveData.Heroes.Where(h => h.OwningState == EHeroOwningState.Unowned).Count(); } }
    public int OwnedHeroCount { get { return _saveData.Heroes.Where(h => h.OwningState == EHeroOwningState.Owned).Count(); } }
    public int PickedHeroCount { get { return _saveData.Heroes.Where(h => h.OwningState == EHeroOwningState.Picked).Count(); } }

    public int GenerateItemDbID()
    {
        int itemDbID = _saveData.ItemDbIDGenerator;
        _saveData.ItemDbIDGenerator++;
        return itemDbID;
    }
    #endregion

    #region Map
    public EGameState GameState { get; private set; } = EGameState.Playing;
    public Stage CurrentStage { get; set; }
    public int CurrentStageIndex { get; set; } = -1;
    #endregion

    #region Teleport
    public void TeleportPlayer(Vector3 teleportPosition)
    {
        Managers.Object.Player.transform.position = teleportPosition; 
    }
    #endregion 

    #region Save & Load
    public string Path { get { return Application.persistentDataPath + "/SaveData.json"; } }
    public void InitGame()
    {
        if (File.Exists(Path))
            return;

        // Hero
        var heroes = Managers.Data.HeroDic.Values.ToList();
        foreach (HeroData hero in heroes)
        {
            HeroSaveData saveData = new HeroSaveData()
            {
                DataID = hero.DataID,
            };

            SaveData.Heroes.Add(saveData);
        }
        // TEMP
        SaveData.Heroes[0].OwningState = EHeroOwningState.Picked;
        SaveData.Heroes[1].OwningState = EHeroOwningState.Owned;
        SaveData.Heroes[2].OwningState = EHeroOwningState.Unowned;
        Money = 100;

        // Quest
        {
            var quests = Managers.Data.QuestDic.Values.ToList();

            foreach (QuestData questData in quests)
            {
                QuestSaveData saveData = new QuestSaveData()
                {
                    TemplateID = questData.DataID,
                    State = EQuestState.None,
                    ProgressCount = new List<int>(),
                    NextResetTime = DateTime.Now,
                };

                for (int i = 0; i < questData.QuestTasks.Count; i++)
                {
                    saveData.ProgressCount.Add(0);
                }

                Debug.Log("SaveDataQuest");
                Managers.Quest.AddQuest(saveData);
            }
        }
    }
    public void SaveGame()
    {
        // Hero
        /*{
            SaveData.Heroes.Clear();
            foreach (HeroInfo heroinfo in Managers.Hero.AllHeroInfos.Values)
            {
                SaveData.Heroes.Add(heroinfo.SaveData);
            }
        }*/

        // Item
        {
            SaveData.Items.Clear();
            foreach(Item item in Managers.Inventory.AllItems)
                SaveData.Items.Add(item.SaveData);
        }

        // Quest
        {
            SaveData.AllQuests.Clear();
            foreach (Quest quest in Managers.Quest.AllQuests.Values)
            {
                SaveData.AllQuests.Add(quest.SaveData);
            }
        }

        string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        File.WriteAllText(Path, jsonStr);
        Debug.Log($"Save Game Completed : {Path}");
    }
    public bool LoadGame()
    {
        if (File.Exists(Path) == false)
            return false;

        string fileStr = File.ReadAllText(Path);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(fileStr);

        if (data != null)
            Managers.Game.SaveData = data;

        // Hero
        {
            Managers.Hero.AllHeroInfos.Clear();

            foreach(HeroSaveData saveData in data.Heroes)
            {
                Managers.Hero.AddHeroInfo(saveData);
            }
            Managers.Hero.AddUnknownHeroes();
        }

        // Item
        {
            Managers.Inventory.Clear();
            foreach(ItemSaveData itemSaveData in data.Items)
            {
                Managers.Inventory.AddItem(itemSaveData);
            }
        }

        // Quest
        {
            Managers.Quest.Clear();

            foreach (QuestSaveData questSaveData in data.AllQuests)
            {
                Managers.Quest.AddQuest(questSaveData);
            }

            Managers.Quest.AddUnknownQuests();
        }

        Debug.Log($"Save Game Loaded : {Path}");
        return true;
    }
    #endregion

    #region Action
    public event Action<EBroadcastEventType, int> OnBroadcastEvent;
    #endregion
}
