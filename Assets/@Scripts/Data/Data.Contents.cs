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
        public string PrefabLabel;
        public string AnimatorName;

        public int DropItemId;

        public float Mass;
        public float MaxHp;
        public float MoveSpeed;
        public float JumpSpeed;
        public float Acceleration;
        public float Deceleration;

        public HitBoxData HitBox;
        public HitBoxData HitCircle;
    }

    #region PlayerData
    [Serializable]
    public class PlayerData : CreatureData
    {
        
    }

    [Serializable]
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        public List<PlayerData> players = new List<PlayerData>();
        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
            foreach (PlayerData player in players)
                dict.Add(player.DataID, player);
            return dict;
        }
    }
    #endregion

    #endregion
}