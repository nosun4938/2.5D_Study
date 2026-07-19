using UnityEngine;
using static Define;

public class Player_Ground : PlayerStateBase
{
    public Player_Ground(Player owner, PlayerStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.Animator.Play("Idle", 0, 0);
    }

    public override void Update()
    {
        base.Update();
        //UpdateAnimation();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Owner.HorizontalMove();
    }

    private float _previousInput = 0f;
    private bool _isStartingRun = false;
    private void UpdateAnimation()
    {
        float input = Owner.Horizontal;
        float velocity = Owner.Rigidbody.linearVelocityX;

        if (_isStartingRun)
        {
            AnimatorStateInfo info = Owner.Animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1f)
                _isStartingRun = false;
            else
                return;
        }

        // Idle -> StartRun
        if (Mathf.Abs(_previousInput) < 0.01f)
        {
            if (input > 0.01f)
            {
                _isStartingRun = true;
                PlayAnimation("Run_Start_R");
                _previousInput = input;
                return;
            }

            if (input < -0.01f)
            {
                _isStartingRun = true;
                PlayAnimation("Run_Start_L");
                _previousInput = input;
                return;
            }
        }

        // Stop
        if (Mathf.Abs(input) < 0.01f)
        {
            if (Mathf.Abs(velocity) < 0.1f)
                PlayAnimation("Idle");
            else
            {
                if (velocity > 0)
                    PlayAnimation("Stop_R");
                else
                    PlayAnimation("Stop_L");
            }
            _previousInput = input;
            return;
        }

        // Turn
        if (input > 0 && velocity < -0.1f)
        {
            PlayAnimation("Turn_L2R");

            _previousInput = input;
            return;
        }

        if (input < 0 && velocity > 0.1f)
        {
            PlayAnimation("Turn_R2L");

            _previousInput = input;
            return;
        }

        // Run
        if (input > 0)
            PlayAnimation("Run_R");
        else
            PlayAnimation("Run_L");

        _previousInput = input;
    }

    private void PlayAnimation(string animName)
    {
        if (CurrentAnimName == animName)
            return;

        CurrentAnimName = animName;
        Owner.Animator.Play(animName, 0, 0);
    }
}
