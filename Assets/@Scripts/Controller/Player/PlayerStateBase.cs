using UnityEngine;
using static Define;

public class PlayerStateBase
{
    protected Player Owner;
    protected PlayerStateMachine _stateMachine;
    protected PlayerStateBase(Player owner, PlayerStateMachine stateMachine)
    {
        Owner = owner;
        _stateMachine = stateMachine;
    }

    public string CurrentAnimName { get; set; }
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        CurrentAnimName = null;
    }
    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {
        
    }
    public virtual void ReEnter()
    {
        Exit();
        Enter();
    }

    #region Animation Helpers
    protected void PlayAnimation(string animName)
    {
        if (CurrentAnimName == animName)
            return;

        CurrentAnimName = animName;
        Owner.Animator.Play(animName, 0, 0f);
    }

    protected bool IsAnimFinished()
    {
        var info = Owner.Animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(CurrentAnimName) && info.normalizedTime >= 1f;
    }
    #endregion
}
