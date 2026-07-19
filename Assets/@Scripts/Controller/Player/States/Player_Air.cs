using UnityEngine;
using static Define;

public class Player_Air : PlayerStateBase
{
    public Player_Air(Player owner, PlayerStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.Animator.Play("Idle", 0, 0);
    }

    public override void Update()
    {
        base.Update();

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Owner.HorizontalMove();
    }
}
