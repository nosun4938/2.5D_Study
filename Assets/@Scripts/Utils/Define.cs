using NUnit.Framework;
using System;
using UnityEngine;

public static class Define
{
    public enum EScene
    {
        Unknown,
        LoadScene,
        TitleScene,
        GameScene,
    }

    public enum EGameState
    {
        Playing,
        Pause,
        CutScene,
        ItemAcquire,
        Dialogue,
        Loading,
    }

    public enum ESound
    {
        Bgm,
        Effect,
        Max,
    }

    public enum EBackgroundType
    {
        Background,
        Middleground,
        Foreground,
    }

    public enum EObjectType
    {
        None,
        Creature,
        Interaction,
        Item,
    }

    public enum EInteractionType
    {
        None,
        NPC,
        Bench,
        Chest,
    }

    public enum ECreatureType
    {
        None,
        Player,
        Monster,
        Boss,
        Artifact,
    }

    public enum EArtifactType
    {
        None,
        Wood,
        Fire,
        Earth,
        Metal,
        Water,
    }

    public enum ECreatureState
    {
        None,
        Idle,
        RunStart,
        RunMid,
        Stop,
        Turn,

        Jump,
        Fall,
        Land,

        Dash,
        Crouch,
        Dead,

        Hitstun,
        Stagger,
        Airborne,
        Knockdown,
    }

    public enum EStateChangeReason
    {
        None,
        Jump,
        Fall,
        Land,
    }

    public enum EMonsterAIState
    {
        Move,
        Evade,
        Attack,
        Cope,
    }

    public enum ESkillSlot
    {
        None,
        Jump,
        Dash,
    }

    public enum ESkillType
    {
        None,
        Hitstun,
        Stagger,
        Airborne,
        Knockdown,
        LastHit,
        WakeUp, // 피격판정 때문에 있는것.
    }

    public enum ESkillEffectType
    {
        None,
        Hit,
        Slash
    }

    public enum ESkillMoveType
    {
        None,
        Dash,
        Step,
        Slide,
        Heavy,
    }

    public enum ECreatureWeapon
    {
        None,
        Barrier,
        Sword,
        Dagger,
        BattleAxe,
        Hammer,
        Shield,
    }

    public static class AnimName
    {
        public const string IDLE = "Idle-Animation";
        public const string RUN = "Run-Animation";
        public const string JUMP = "Jump-Animation";
        public const string CROUCH = "Crouch-Animation";
        public const string Hurt = "Hurt-Animation";
        public const string Skill = "Crouch-Animation";
        public const string Dead = "Dead-Animation";
    }

    public static class SortingLayers
    {
        public const int SPELL_INDICATOR = 200;
        public const int ENV = 270;
        public const int NPC = 280;
        public const int BOSS = 290;
        public const int HERO = 300;
        public const int ITEM = 310;
        public const int MONSTER = 320;
        public const int ARTIFACT = 330;
        public const int COMBAT_UI = 400;
        public const int SKILL_EFFECT = 410;
        public const int DAMAGE_FONT = 420;
        public const int MAP_FOG = 1000;
    }

    public static class PlayerMoveConst
    {
        public const float GROUND_CHECK_DISTANCE = 0.2f;
        public const float JUMP_FORCE = 50.0f;
        public const int MAX_JUMP_COUNT = 2;
        public const float COYOTE_TIME = 0.05f;
        public const float ACCELERATION = 50f;
        public const float DECELERATION = 100f;
    }

    [Serializable]
    public class HitCircleData
    {
        public Vector2 Offset;
        public float Radius;
        public string TargetLayer;
    }

    [Serializable]
    public class HitBoxData
    {
        public Vector2 Offset;
        public Vector2 Size;
        public string TargetLayer;
    }

    [Serializable]
    public class CreatureSkillMap
    {
        public StateSkill Default;
        public StateSkill HeroSkill;
        public StateSkill MonsterSkill;
        public StateSkill BossSkill;
        public StateSkill ArtifactSkill;

        public StateSkill GetStateSkill(ECreatureType type)
        {
            switch (type)
            {
                case ECreatureType.Player:
                    return HeroSkill ?? Default;

                case ECreatureType.Monster:
                    return MonsterSkill ?? Default;

                case ECreatureType.Boss:
                    return BossSkill ?? Default;

                case ECreatureType.Artifact:
                    return ArtifactSkill ?? Default;

                default:
                    return Default;
            }
        }

        public int GetSkillId(ECreatureType type, ECreatureState state)
        {
            StateSkill skill = GetStateSkill(type);

            if (skill == null)
                return 0;

            switch (state)
            {
                case ECreatureState.Idle:
                    return skill.Idle;

                case ECreatureState.Jump:
                    return skill.Jump;

                case ECreatureState.Dash:
                    return skill.Dash;

                default:
                    return 0;
            }
        }
    }

    [Serializable]
    public class StateSkill
    {
        public int Dash;
        public int Idle;
        public int Jump;
    }

    [Serializable]
    public class SpawnData
    {
        public string SpawnPointID;
        public EObjectType ObjectType;
        public int DataId;
        public Vector3Int Position;
    }

    [Serializable]
    public class WorldObjectSaveData
    {
        public string Id;
        public bool Active;
        public Vector3 Position;
        public int Hp;
    }

    public struct SkillKey : IEquatable<SkillKey>
    {
        public ECreatureWeapon Weapon;
        public ECreatureState State;
        public ESkillSlot Slot;
        public int Combo;

        public SkillKey(ECreatureWeapon weapon, ECreatureState state, ESkillSlot slot, int combo)
        {
            Weapon = weapon;
            State = state;
            Slot = slot;
            Combo = combo;
        }

        public override string ToString()
        {
            return $"Weapon:{Weapon}, State:{State}, Slot:{Slot}, Combo:{Combo}";
        }

        public bool Equals(SkillKey other) =>
            Weapon == other.Weapon && State == other.State && Slot == other.Slot && Combo == other.Combo;

        public override int GetHashCode() =>
            HashCode.Combine((int)Weapon, (int)State, (int)Slot, (int)Combo);

        public override bool Equals(object obj) =>
            obj is SkillKey other && Equals(other);
    }
}
