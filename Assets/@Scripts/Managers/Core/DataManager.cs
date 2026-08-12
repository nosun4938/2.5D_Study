using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.HeroData> HeroDic { get; private set; } = new Dictionary<int, Data.HeroData>();
    public Dictionary<int, Data.NpcData> NpcDic { get; private set; } = new Dictionary<int, Data.NpcData>();
    public Dictionary<int, Data.ItemData> ItemDic { get; private set; } = new Dictionary<int, Data.ItemData>();
    public Dictionary<string, Data.TextData> TextDic { get; private set; } = new Dictionary<string, Data.TextData>();

    public void Init()
    {
        HeroDic = LoadJson<Data.HeroDataLoader, int, Data.HeroData>("HeroData").MakeDict();
        NpcDic = LoadJson<Data.NpcDataLoader, int, Data.NpcData>("NpcData").MakeDict();
        ItemDic = LoadJson<Data.ItemDataLoader, int, Data.ItemData>("ItemData").MakeDict();
        TextDic = LoadJson<Data.TextDataLoader, string, Data.TextData>("TextData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
