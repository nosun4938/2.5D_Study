using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static Define;
using static UnityEngine.UI.GridLayoutGroup;

public class Player_Air : PlayerStateBase
{
    private Vector3 JumpCutMultiplier = new Vector3(1, 0.5f, 1);

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
    const float _fullJumpTime = 0.35f;
    float _timer = 0f;
    private void EnterJump()
    {
        //Debug.Log("Enter Jump");
        Owner.HasJumped = true;
        Owner.Rigidbody.SetVelocityY(Owner.JumpSpeed);

        _timer = 0f;

        Owner.CreatureState = ECreatureState.Jump;
        PlayAnimation("Jump");
    }

    private void UpdateJump()
    {
        _timer += Time.deltaTime;

        if (Owner.IsJumpPressed == false && Owner.Rigidbody.linearVelocity.y > 0f)
        {
            Owner.Rigidbody.linearVelocity = Vector3.Scale(Owner.Rigidbody.linearVelocity, JumpCutMultiplier);
        }

        // To Fall
        if (Owner.Rigidbody.linearVelocity.y < 0f)
        {
            EnterFall();
            return;
        }
        
        // To Fall_Timeover
        if (_timer > _fullJumpTime)
        {
            Owner.Rigidbody.linearVelocity = Vector3.Scale(Owner.Rigidbody.linearVelocity, JumpCutMultiplier);
            EnterFall();
            return;
        }

        Owner.LookDirection();
    }
    #endregion

    #region Fall
    private void EnterFall()
    {
        //Debug.Log("Enter Fall");
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
        else
        {
            Owner.Rigidbody.AddForce(Physics.gravity * 4);
        }

        Owner.LookDirection();
    }
    #endregion
}
