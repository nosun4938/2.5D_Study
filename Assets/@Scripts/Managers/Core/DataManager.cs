using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
    bool Validate();
}

public class DataManager
{
    public Dictionary<int, Data.HeroData> HeroDic { get; private set; } = new Dictionary<int, Data.HeroData>();
    public Dictionary<int, Data.MonsterData> MonsterDic { get; private set; } = new Dictionary<int, Data.MonsterData>();
    public Dictionary<int, Data.NpcData> NpcDic { get; private set; } = new Dictionary<int, Data.NpcData>();
    public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.EffectData> EffectDic { get; private set; } = new Dictionary<int, Data.EffectData>();
    public Dictionary<string, Data.TextData> TextDic { get; private set; } = new Dictionary<string, Data.TextData>();

    public Dictionary<int, Data.ItemData> ItemDic { get; private set; } = new Dictionary<int, Data.ItemData>();
    public Dictionary<int, Data.ColleagueData> ColleagueDic { get; private set; } = new Dictionary<int, Data.ColleagueData>();
    public Dictionary<int, Data.SkillBookData> SkillBookDic { get; private set; } = new Dictionary<int, Data.SkillBookData>();
    public Dictionary<int, Data.ConsumableData> ConsumableDic { get; private set; } = new Dictionary<int, Data.ConsumableData>();

    public Dictionary<int, Data.QuestData> QuestDic { get; private set; } = new Dictionary<int, Data.QuestData>();

    public void Init()
    {
        Data.HeroDataLoader heroDataLoader = LoadJson<Data.HeroDataLoader, int, Data.HeroData>("HeroData");
        HeroDic = heroDataLoader.MakeDict();
        
        MonsterDic = LoadJson<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData").MakeDict();
        NpcDic = LoadJson<Data.NpcDataLoader, int, Data.NpcData>("NpcData").MakeDict();
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData").MakeDict();
        EffectDic = LoadJson<Data.EffectDataLoader, int, Data.EffectData>("EffectData").MakeDict();
        TextDic = LoadJson<Data.TextDataLoader, string, Data.TextData>("TextData").MakeDict();
        QuestDic = LoadJson<Data.QuestDataLoader, int, Data.QuestData>("QuestData").MakeDict();

        ColleagueDic = LoadJson<Data.ItemDataLoader<Data.ColleagueData>, int, Data.ColleagueData>("Item_ColleagueData").MakeDict();
        SkillBookDic = LoadJson<Data.ItemDataLoader<Data.SkillBookData>, int, Data.SkillBookData>("Item_SkillBookData").MakeDict();
        ConsumableDic = LoadJson<Data.ItemDataLoader<Data.ConsumableData>, int, Data.ConsumableData>("Item_ConsumableData").MakeDict();

        ItemDic.Clear();
        foreach (var item in ColleagueDic)
            ItemDic.Add(item.Key, item.Value);
        foreach (var item in SkillBookDic)
            ItemDic.Add(item.Key, item.Value);
        foreach (var item in ConsumableDic)
            ItemDic.Add(item.Key, item.Value);

        // Validation
        if (heroDataLoader.Validate())
            Debug.Log("Hero Data Text Validate");
        else
            Debug.Log("Hero Data Text Missing");
        
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
