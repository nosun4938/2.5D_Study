using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class EffectBase : BaseObject
{
    public Creature Owner;
    public SkillBase Skill;
    public EffectData EffectData;
    public EEffectType EffectType;

    protected EEffectSpawnType _spawnType;
    protected float Remains { get; set; } = 0;
    protected bool Loop { get; set; } = true;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(int templateID, Creature owner, EEffectSpawnType spawnType, SkillBase skill)
    {
        DataTemplateID = templateID;
        EffectData = Managers.Data.EffectDic[templateID];

        Skill = skill;
        Owner = owner;
        _spawnType = spawnType;

        // Layer
        SpriteRenderer.sortingOrder = SortingLayers.SKILL_EFFECT;

        // Animator
        Animator.runtimeAnimatorController = Managers.Resource.Load<RuntimeAnimatorController>("EffectAnimation");

        // EffectType
        EffectType = EffectData.EffectType;
    }

    public virtual void ApplyEffect()
    {
        ShowEffect();
    }

    protected virtual void ShowEffect()
    {
        PlayAnimation(AnimName.IDLE);
    }

    public virtual bool ClearEffect(EEffectClearType clearType)
    {
        Debug.Log($"ClearEffect - {gameObject.name} {EffectData.ClassName} -> {clearType}");

        switch (clearType)
        {
            case EEffectClearType.TimeOut:
            case EEffectClearType.End:
                Managers.Object.Despawn(this);
                return true;
        }

        return false;
    }
}
