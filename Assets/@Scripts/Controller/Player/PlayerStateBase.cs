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

    
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        Owner.CurrentAnimName = null;
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
