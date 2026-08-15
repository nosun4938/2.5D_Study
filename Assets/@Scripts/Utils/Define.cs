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
        Player,
        Monster,
        Boss,
        Npc,
        Item,
    }

    public enum ENpcType
    {
        None,
        Dummy,
        Guild,
        Weapon,
        Waypoint,
    }
    public enum EUIEvent
    {
        Click,
        PointerDown,
        PointerUp,
        Drag,
    }

    public enum ECreatureState
    {
        None,
        Idle,
        RunStart,
        RunMid,
        Stop,
        Turn,
        Dead,

        Jump,
        Fall,
        Land,

        Skill,
    }

    public enum EStateChangeReason
    {
        None,
        Jump,
        Fall,
        Land,

        NormalAtk,
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
        NormalAtk,
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

    public enum ELanguage
    {
        Korean,
        English
    }

    public static class AnimName
    {
        public const string IDLE = "Idle";
        public const string RUNSTART = "Run_Start";
        public const string RUNMID = "Run_Mid";
        public const string STOP = "Stop";
        public const string TURN = "Turn";
        public const string JUMP = "Jump";
        public const string FALL = "Fall";
        public const string LAND = "Land";
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
        public Vector3 Offset;
        public Vector3 Size;
        public string TargetLayer;
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
}
