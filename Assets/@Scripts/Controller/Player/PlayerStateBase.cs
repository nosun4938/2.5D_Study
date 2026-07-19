using UnityEngine;

public class PlayerStateBase
{
    protected Player Owner;
    protected PlayerStateMachine _stateMachine;
    protected PlayerStateBase(Player owner, PlayerStateMachine stateMachine)
    {
        Owner = owner;
        _stateMachine = stateMachine;
    }

    protected string CurrentAnimName { get; set; }
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        Owner.Rigidbody.gravityScale = 3.0f;
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
}
