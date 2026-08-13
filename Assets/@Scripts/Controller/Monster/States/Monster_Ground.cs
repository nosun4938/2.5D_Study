using UnityEngine;
using static Define;
using static UnityEngine.UI.GridLayoutGroup;

public class Monster_Ground : MonsterStateBase
{
    public Monster_Ground(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();

        EnterIdle();
    }

    public override void Update()
    {
        base.Update();

        switch (Owner.CreatureState)
        {
            case ECreatureState.Idle: UpdateIdle(); break;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Owner.HorizontalMove();
    }

    #region Idle
    private void EnterIdle()
    {
        //Debug.Log("Enter Idle");
        Owner.CreatureState = ECreatureState.Idle;
    }

    private void UpdateIdle()
    {
        if (Owner.Horizontal != 0)
        {
            return;
        }
        Owner.LookDirection();
    }
    #endregion
}
