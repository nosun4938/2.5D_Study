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

    public List<HeroSaveData> Heroes = new List<HeroSaveData>();
}

[Serializable]
public class HeroSaveData
{
    public int Level = 1;
    public int Exp = 10;

    public int DataId = 0;
    public HeroOwningState OwningState = HeroOwningState.Unowned;
}

public enum HeroOwningState
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

    public List<HeroSaveData> AllHeroes { get { return _saveData.Heroes; } }
    public int TotalHeroCount { get { return _saveData.Heroes.Count; } }
    public int UnownedHeroCount { get { return _saveData.Heroes.Where(h => h.OwningState == HeroOwningState.Unowned).Count(); } }
    public int OwnedHeroCount { get { return _saveData.Heroes.Where(h => h.OwningState == HeroOwningState.Owned).Count(); } }
    public int PickedHeroCount { get { return _saveData.Heroes.Where(h => h.OwningState == HeroOwningState.Picked).Count(); } }
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

        var heroes = Managers.Data.HeroDic.Values.ToList();
        foreach (HeroData hero in heroes)
        {
            HeroSaveData saveData = new HeroSaveData()
            {
                DataId = hero.DataID,
            };

            SaveData.Heroes.Add(saveData);
        }

        // TEMP
        SaveData.Heroes[0].OwningState = HeroOwningState.Picked;
        SaveData.Heroes[1].OwningState = HeroOwningState.Owned;
        SaveData.Heroes[2].OwningState = HeroOwningState.Unowned;
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
