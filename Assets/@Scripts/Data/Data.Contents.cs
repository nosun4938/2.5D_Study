using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

#region DataID 기준
// 수정 필요

// Creature     십만번대
// - Artifact   100,000 번대
// - Hero       200,000 번대
// - Monster    300,000 번대
// - Boss       500,000 번대

// Skill        만번대
// - None       10,000 번대
// - Sword      20,000 번대
// - Dagger     30,000
// - GreatSword 40,000
// - BattleAxe  50,000
// - Shield     60,000
#endregion

namespace Data
{
    #region CreatureData
    [Serializable]
    public class CreatureData
    {
        public int DataID;
        public string DescriptionTextID;
        public string IconImage;
        public string PrefabLabel;
        public string AnimatorName;

        public int DropItemID;

        public float Mass;
        public float MaxHp;
        public float MoveSpeed;
        public float JumpSpeed;
        public float Acceleration;
        public float Deceleration;

        public int NormalAtkID;

        public HitBoxData HitBox;
        public HitBoxData HitCircle;
    }

    #region HeroData
    [Serializable]
    public class HeroData : CreatureData
    {

    }

    [Serializable]
    public class HeroDataLoader : ILoader<int, HeroData>
    {
        public List<HeroData> heroes = new List<HeroData>();
        public Dictionary<int, HeroData> MakeDict()
        {
            Dictionary<int, HeroData> dict = new Dictionary<int, HeroData>();
            foreach (HeroData hero in heroes)
                dict.Add(hero.DataID, hero);
            return dict;
        }
    }
    #endregion

    #region MonsterData
    [Serializable]
    public class MonsterData : CreatureData
    {

    }

    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();
        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in monsters)
                dict.Add(monster.DataID, monster);
            return dict;
        }
    }
    #endregion
    #endregion

    #region NpcData
    [Serializable]
    public class NpcData
    {
        public int DataID;
        public string DescriptionTextID;
        public ENpcType NpcType;
        public string PrefabLabel;
        public string AnimatorName;
        public HitBoxData InteractionBox;
    }

    [Serializable]
    public class NpcDataLoader : ILoader<int, NpcData>
    {
        public List<NpcData> npcs = new List<NpcData>();
        public Dictionary<int, NpcData> MakeDict()
        {
            Dictionary<int, NpcData> dict = new Dictionary<int, NpcData>();
            foreach (NpcData npc in npcs)
                dict.Add(npc.DataID, npc);
            return dict;
        }
    }
    #endregion

    #region ItemData
    [Serializable]
    public class ItemData
    {
        public int DataID;
        public EItemGroupType ItemGroupType;
        public EItemType ItemType;
        public EItemSubType ItemSubType;
        public EItemGrade Grade;

        public string DescriptionTextID;
        public string PrefabLabel;
        public string AnimatorName;
        public HitBoxData InteractionBox;
    }

    public class ColleagueData : ItemData
    {

    }

    public class SkillBookData : ItemData
    {

    }

    public class ConsumableData : ItemData
    {
        public int Value;
    }

    [Serializable]
    public class ItemDataLoader<T> : ILoader<int, T> where T : ItemData
    {
        public List<T> items = new List<T>();
        public Dictionary<int, T> MakeDict()
        {
            Dictionary<int, T> dict = new Dictionary<int, T>();
            foreach (T item in items)
                dict.Add(item.DataID, item);
            return dict;
        }
    }
    #endregion

    #region SkillData
    [Serializable]
    public class SkillData
    {
        public int DataID;
        public string Name;
        public string ClassName;
        public string Description;
        public string IconLabel;
        public string AnimName;
        public float Duration;
        public float CoolTime;
        public float Damage;
        public float DamageMultiplier;
        public string SkillSound;
        public List<int> EffectIDs = new List<int>();
        public float EffectSize;
        public HitBoxData HitBox;
    }

    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();
        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
            foreach (SkillData skill in skills)
                dict.Add(skill.DataID, skill);
            return dict;
        }
    }
    #endregion

    #region EffectData
    [Serializable]
    public class EffectData
    {
        public int DataID;
        public string Name;
        public string ClassName;
        public string Description;
        public string AnimName;
        public string IconLabel;
        public string EffectSound;
        public EEffectType EffectType;
    }

    [Serializable]
    public class EffectDataLoader : ILoader<int, EffectData>
    {
        public List<EffectData> effects = new List<EffectData>();
        public Dictionary<int, EffectData> MakeDict()
        {
            Dictionary<int, EffectData> dict = new Dictionary<int, EffectData>();
            foreach (EffectData effect in effects)
                dict.Add(effect.DataID, effect);
            return dict;
        }
    }
    #endregion

    #region TextData
    [Serializable]
    public class TextData
    {
        public string DataID;
        public string KOR;
        public string ENG;
    }

    [Serializable]
    public class TextDataLoader : ILoader<string, TextData>
    {
        public List<TextData> texts = new List<TextData>();
        public Dictionary<string, TextData> MakeDict()
        {
            Dictionary<string, TextData> dict = new Dictionary<string, TextData>();
            foreach (TextData text in texts)
                dict.Add(text.DataID, text);
            return dict;
        }
    }

    #endregion
}