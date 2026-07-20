using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class Player_Ground : PlayerStateBase
{
    public Player_Ground(Player owner, PlayerStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();

        if (Owner.ChangeReason == EStateChangeReason.Land)
            EnterLand();
        else
            EnterIdle();
    }

    public override void Update()
    {
        base.Update();

        if (!Owner.IsGrounded)
        {
            Owner.ChangeReason = EStateChangeReason.Fall;
            _stateMachine.ChangeState(Owner._airState);
        }

        switch (Owner.CreatureState)
        {
            case ECreatureState.Idle: UpdateIdle(); break;
            case ECreatureState.RunStart: UpdateRunStart(); break;
            case ECreatureState.RunMid: UpdateRunMid(); break;
            case ECreatureState.Stop: UpdateStop(); break;
            case ECreatureState.Turn: UpdateTurn(); break;
            case ECreatureState.Land: UpdateLand(); break;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Owner.HorizontalMove();
    }


    // Sub States
    #region Input helpers
    private bool HasInput => Mathf.Abs(Owner.Horizontal) > 0;
    private bool InputRight => Owner.Horizontal > 0;
    private bool IsOppositeInput => HasInput && (InputRight != Owner.LookRight);
    #endregion
    
    #region Idle
    private void EnterIdle()
    {
        Debug.Log("Enter Idle");
        Owner.CreatureState = ECreatureState.Idle;
        PlayAnimation("Idle");
    }

    private void UpdateIdle()
    {
        if (Owner.Horizontal != 0)
        {
            EnterRunStart();
            return;
        }
        Owner.LookDirection();
    }
    #endregion

    #region RunStart
    private void EnterRunStart()
    {
        Debug.Log("Enter RunStart");
        Owner.CreatureState = ECreatureState.RunStart;
        PlayAnimation("Run_Start");
    }

    private void UpdateRunStart()
    {
        // To Turn
        if (IsOppositeInput)
        {
            EnterTurn();
            return;
        }

        // To Stop
        if (HasInput == false)
        {
            EnterStop();
            return;
        }

        // To RunMid
        if (IsAnimFinished())
        {
            EnterRunMid();
            return;
        }

        Owner.LookDirection();
    }
    #endregion

    #region RunMid
    private void EnterRunMid()
    {
        Debug.Log("Enter RunMid");
        Owner.CreatureState = ECreatureState.RunMid;
        PlayAnimation("Run_Mid");
    }

    private void UpdateRunMid()
    {
        // To Turn
        if (IsOppositeInput)
        {
            EnterTurn();
            return;
        }

        // To Stop
        if (HasInput == false)
        {
            EnterStop();
            return;
        }

        Owner.LookDirection();
    }
    #endregion

    #region Stop
    private void EnterStop()
    {
        Debug.Log("Enter Stop");
        Owner.CreatureState = ECreatureState.Stop;
        PlayAnimation("Stop");
    }

    private void UpdateStop()
    {
        // To RunStart
        if (HasInput && InputRight == Owner.LookRight)
        {
            EnterRunStart();
            return;
        }

        // To Turn
        if (IsOppositeInput)
        {
            EnterTurn();
            return;
        }

        if (IsAnimFinished())
        {
            EnterIdle();
            return;
        }

        Owner.LookDirection();
    }
    #endregion

    #region Turn
    private void EnterTurn()
    {
        Debug.Log("Enter Turn");
        Owner.CreatureState = ECreatureState.Turn;
        PlayAnimation("Turn");
    }

    private void UpdateTurn()
    {
        if (IsAnimFinished() == false)
            return;

        // To RunMid
        if (HasInput && InputRight != Owner.LookRight)
        {
            Owner.LookRight = !Owner.LookRight;
            EnterRunMid();
            return;
        }
        // To Stop
        else
        {
            Owner.LookRight = !Owner.LookRight;
            EnterStop();
            return;
        }
    }
    #endregion

    #region Land
    private void EnterLand()
    {
        Debug.Log("Enter Land");
        Owner.CreatureState = ECreatureState.Land;
        PlayAnimation("Land");
    }

    private void UpdateLand()
    {
        // To RunStart
        if (Owner.Horizontal != 0)
        {
            EnterRunStart();
            return;
        }
        
        // To Idle
        if (IsAnimFinished() == true)
        {
            EnterIdle();
            return;
        }

        Owner.LookDirection();
    }
    #endregion
}
