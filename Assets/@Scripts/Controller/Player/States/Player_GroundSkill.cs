using UnityEngine;
using static Define;

public class Player_GroundSkill : PlayerStateBase
{
    SkillBase _nowSkill;
    public Player_GroundSkill(Player owner, PlayerStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.Rigidbody.linearVelocity = Vector3.zero;
        if (Owner.Horizontal != 0)
        {
            Owner.LookRight = Owner.Horizontal > 0;
        }


        Owner.CreatureState = ECreatureState.Skill;

        switch (Owner.ChangeReason)
        {
            case (EStateChangeReason.NormalAtk):
                _nowSkill = Owner.Skills.NormalAtk;
                _nowSkill.DoSkill();
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        if (_nowSkill.IsFinished)
        {
            _stateMachine.ChangeState(Owner.IsGrounded ? Owner._groundState : Owner._airState);
            return;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //Owner.HorizontalMove();
    }
}
