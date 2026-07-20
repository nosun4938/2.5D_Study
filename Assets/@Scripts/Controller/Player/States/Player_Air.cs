using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static Define;
using static UnityEngine.UI.GridLayoutGroup;

public class Player_Air : PlayerStateBase
{
    private const float JumpCutMultiplier = 0.5f;

    public Player_Air(Player owner, PlayerStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();

        if (Owner.ChangeReason == EStateChangeReason.Jump)
            EnterJump();
        else if (Owner.ChangeReason == EStateChangeReason.Fall)
            EnterFall();
        else
            EnterFall();
    }

    public override void Update()
    {
        base.Update();

        switch (Owner.CreatureState)
        {
            case ECreatureState.Jump: UpdateJump(); break;
            case ECreatureState.Fall: UpdateFall(); break;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Owner.HorizontalMove();
    }

    #region Jump
    private void EnterJump()
    {
        Debug.Log("Enter Jump");
        Owner.HasJumped = true;
        Owner.Rigidbody.linearVelocityY = Owner.JumpSpeed;
        Owner.Rigidbody.gravityScale = 3.0f;

        Owner.CreatureState = ECreatureState.Jump;
        PlayAnimation("Jump");
    }

    private void UpdateJump()
    {
        if (Owner.IsJumpPressed == false && Owner.Rigidbody.linearVelocityY > 0f)
        {
            Owner.Rigidbody.linearVelocityY *= JumpCutMultiplier;
        }

        // To Fall
        if (Owner.Rigidbody.linearVelocityY < 0f)
        {
            EnterFall();
            return;
        }

        Owner.LookDirection();
    }
    #endregion

    #region Fall
    private void EnterFall()
    {
        Debug.Log("Enter Fall");
        Owner.Rigidbody.gravityScale = 5.0f;
        Owner.CreatureState = ECreatureState.Fall;
        PlayAnimation("Fall");
    }

    private void UpdateFall()
    {
        // To Land
        if (Owner.IsGrounded)
        {
            Owner.ChangeReason = EStateChangeReason.Land;
            _stateMachine.ChangeState(Owner._groundState);
            return;
        }

        Owner.LookDirection();
    }
    #endregion
}
