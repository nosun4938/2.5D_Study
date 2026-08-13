using Data;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public Data.MonsterData MonsterData { get; private set; }

    public override ECreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            if (_creatureState != value)
            {
                base.CreatureState = value;
                UpdateAnimation();
            }
        }
    }

    #region StateMachine
    MonsterStateMachine _stateMachine;
    public EStateChangeReason ChangeReason { get; set; }

    // Movements
    public Monster_Ground _groundState { get; private set; }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;

        // StateMachine
        _stateMachine = new MonsterStateMachine(this);
        _groundState = new(this, _stateMachine);

        return true;
    }

    public override void Update()
    {
        base.Update();

        _stateMachine?.Update();
    }

    public void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        MonsterData = CreatureData as MonsterData;

        // State Machine
        _stateMachine.ChangeState(_groundState);
    }

    #region Animation Helpers
    public string CurrentAnimName { get; set; }
    protected void PlayAnimation(string animName)
    {
        if (CurrentAnimName == animName)
            return;

        CurrentAnimName = animName;
        Animator.Play(animName, 0, 0f);
    }

    public bool IsAnimFinished()
    {
        var info = Animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(CurrentAnimName) && info.normalizedTime >= 1f;
    }
    protected override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                PlayAnimation(AnimName.IDLE);
                break;
            case ECreatureState.RunStart:
                PlayAnimation(AnimName.RUNSTART);
                break;
            case ECreatureState.RunMid:
                PlayAnimation(AnimName.RUNMID);
                break;
            case ECreatureState.Stop:
                PlayAnimation(AnimName.STOP);
                break;
            case ECreatureState.Turn:
                PlayAnimation(AnimName.TURN);
                break;
            case ECreatureState.Jump:
                PlayAnimation(AnimName.JUMP);
                break;
            case ECreatureState.Fall:
                PlayAnimation(AnimName.FALL);
                break;
            case ECreatureState.Land:
                PlayAnimation(AnimName.LAND);
                break;

            default:
                break;
        }
    }
    #endregion
}
