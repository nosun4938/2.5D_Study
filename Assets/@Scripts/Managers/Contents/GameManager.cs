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

    public List<ItemSaveData> Items = new List<ItemSaveData>();
}

[Serializable]
public class ItemSaveData
{
    public int DataId = 0;
    public ItemOwningState OwningState = ItemOwningState.Unowned;
}

public enum ItemOwningState
{
    Unowned,
    Owned,
    Picked,
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
            _saveData.Money = value;
            //(Managers.UI.SceneUI as UI_GameScene)?.RefreshMoneyText();
        }
    }

    public List<ItemSaveData> AllItems { get { return _saveData.Items; } }
    public int TotalItemCount { get { return _saveData.Items.Count; } }
    public int UnownedItemCount { get { return _saveData.Items.Where(h => h.OwningState == ItemOwningState.Unowned).Count(); } }
    public int OwnedItemCount { get { return _saveData.Items.Where(h => h.OwningState == ItemOwningState.Owned).Count(); } }
    public int PickedItemCount { get { return _saveData.Items.Where(h => h.OwningState == ItemOwningState.Picked).Count(); } }
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

        var items = Managers.Data.ItemDic.Values.ToList();
        foreach (ItemData item in items)
        {
            ItemSaveData saveData = new ItemSaveData()
            {
                DataId = item.DataID,
            };

            SaveData.Items.Add(saveData);
        }

        // TEMP
        SaveData.Items[0].OwningState = ItemOwningState.Picked;
        SaveData.Items[1].OwningState = ItemOwningState.Unowned;
    }
    public void SaveGame()
    {
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

        Debug.Log($"Save Game Loaded : {Path}");
        return true;
    }
    #endregion
}
