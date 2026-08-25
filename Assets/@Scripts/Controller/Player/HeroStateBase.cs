using UnityEngine;
using static Define;

public class HeroStateBase
{
    protected Hero Owner;
    protected HeroStateMachine _stateMachine;
    protected HeroStateBase(Hero owner, HeroStateMachine stateMachine)
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
