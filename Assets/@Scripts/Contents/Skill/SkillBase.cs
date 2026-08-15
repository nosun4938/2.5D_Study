using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public abstract class SkillBase : InitBase
{
    public Creature Owner { get; protected set; }
    public float RemainCoolTime { get; set; }

    public bool IsFinished { get; protected set; }
    private Coroutine _skillDuration;
    private bool performingHit;

    public Data.SkillData SkillData { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(Creature owner, int skillTemplateID)
    {
        Owner = owner;
        SkillData = Managers.Data.SkillDic[skillTemplateID];
    }

    private void Update()
    {
        if (Owner.PlayingSkill != this)
            return;

        if (performingHit)
        {
            PerformHitDetection();
        }
    }

    public virtual void DoSkill()
    {
        IsFinished = false;

        // 준비된 스킬에서 해제
        if (Owner.Skills != null)
        {
            Owner.Skills.ActiveSkills.Remove(this);
            Owner.PlayingSkill = this;
        }
        Owner.PlayAnimation(SkillData.AnimName);

        _skillDuration = StartCoroutine(CoSkillDuration());
        StartCoroutine(CoCountdownCooldown());
    }

    private IEnumerator CoSkillDuration()
    {
        float duration = SkillData.Duration > 0f
            ? SkillData.Duration
            : Owner.GetAnimClipLength(SkillData.AnimName);

        yield return new WaitForSeconds(duration);
        EndSkill();
    }
    private IEnumerator CoCountdownCooldown()
    {
        RemainCoolTime = SkillData.CoolTime;
        yield return new WaitForSeconds(SkillData.CoolTime);
        RemainCoolTime = 0;

        // 준비된 스킬에 추가
        if (Owner.Skills != null)
            Owner.Skills.ActiveSkills.Add(this);
    }

    protected virtual void EndSkill()
    {
        IsFinished = true;
        Owner.PlayingSkill = null;
    }

    public virtual void CancelSkill()
    {
        if (_skillDuration != null)
            StopCoroutine(_skillDuration);

        EndSkill();
    }



    private void OnOwnerAnimEventHandler(string eventName = null)
    {
        OnAttackEvent();
    }

    protected abstract void OnAttackEvent();

    #region Animation Event
    protected virtual void EnableHit()
    {
        if (Owner.PlayingSkill != this)
            return;
        performingHit = true;
    }

    protected virtual void DisableHit()
    {
        ResetHitTargets();
        performingHit = false;
    }
    #endregion

    #region OnHit
    private HashSet<Creature> _alreadyHit = new HashSet<Creature>();
    protected virtual bool PerformHitDetection()
    {
        Vector3 center = (Vector3)Owner.transform.position + new Vector3(
            Owner.LookRight ? SkillData.HitBox.Offset.x : -SkillData.HitBox.Offset.x,
            SkillData.HitBox.Offset.y
        );
        Vector3 size = SkillData.HitBox.Size;

        Collider[] results = Physics.OverlapBox(center, size, Quaternion.identity, LayerMask.GetMask("HitCircle"));

        foreach (Collider collider in results)
        {
            Creature target = collider.GetComponentInParent<Creature>();
            if (target != null && target != Owner && _alreadyHit.Contains(target) == false)
            {
                Debug.Log($"Hit: {target.name} / Animation: {SkillData.AnimName}");
                target.OnDamaged(Owner, this);
                _alreadyHit.Add(target);
                return true;
            }
        }
        return false;
    }

    protected void ResetHitTargets()
    {
        _alreadyHit.Clear();
    }

    protected virtual void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (Owner == null || SkillData == null)
            return;

        if (IsFinished)
            return;

        if (Owner.PlayingSkill != this)
            return;

        Vector3 center = (Vector3)transform.position + new Vector3(
            Owner?.LookRight == true ? SkillData.HitBox.Offset.x : -SkillData.HitBox.Offset.x,
            SkillData.HitBox.Offset.y
        );

        Vector3 size = SkillData.HitBox.Size;
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(center, size);
#endif
    }
    #endregion
}
