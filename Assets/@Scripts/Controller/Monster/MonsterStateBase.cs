using UnityEngine;

public class MonsterStateBase
{
    protected Monster Owner;
    protected MonsterStateMachine _stateMachine;
    protected MonsterStateBase(Monster owner, MonsterStateMachine stateMachine)
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
