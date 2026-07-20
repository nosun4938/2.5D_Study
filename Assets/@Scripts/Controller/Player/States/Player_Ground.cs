using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class Player_Ground : PlayerStateBase
{
    public Player_Ground(Player owner, PlayerStateMachine stateMachine) : base(owner, stateMachine) { }
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
            case ECreatureState.RunStart: UpdateRunStart(); break;
            case ECreatureState.RunMid: UpdateRunMid(); break;
            case ECreatureState.Stop: UpdateStop(); break;
            case ECreatureState.Turn: UpdateTurn(); break;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Owner.HorizontalMove();
    }


    // Sub States
    #region Input helpers
    private const float InputDeadZone = 0.01f;
    private bool HasInput => Mathf.Abs(Owner.Horizontal) > InputDeadZone;
    private bool InputRight => Owner.Horizontal > InputDeadZone;
    private bool InputLeft => Owner.Horizontal < -InputDeadZone;
    private bool IsOppositeInput => HasInput && (InputRight != Owner.LookRight);
    #endregion
    
    #region Idle
    private void EnterIdle()
    {
        Owner.CreatureState = ECreatureState.Idle;
        PlayAnimation("Idle");
    }

    private void UpdateIdle()
    {
        if (Owner.Horizontal != 0)
        {
            Owner.LookRight = Owner.Horizontal > 0;
            EnterRunStart();
            return;
        }
    }
    #endregion

    #region RunStart
    private void EnterRunStart()
    {
        Owner.CreatureState = ECreatureState.RunStart;
        PlayAnimation("Run_Start");
    }

    private void UpdateRunStart()
    {
        if (Owner.Horizontal != 0)
            Owner.LookRight = Owner.Horizontal > 0;

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
            EnterRunMid();
    }
    #endregion

    #region RunMid
    private void EnterRunMid()
    {
        Owner.CreatureState = ECreatureState.RunMid;
        PlayAnimation("Run_Mid");
    }

    private void UpdateRunMid()
    {
        if (Owner.Horizontal != 0)
            Owner.LookRight = Owner.Horizontal > 0;

        // To Turn
        if (IsOppositeInput)
        {
            EnterTurn();
            return;
        }

        // To Stop
        if (HasInput == false)
            EnterStop();
    }
    #endregion

    #region Stop
    private void EnterStop()
    {
        Owner.CreatureState = ECreatureState.Stop;
        PlayAnimation("Stop");
    }

    private void UpdateStop()
    {
        if (Owner.Horizontal != 0)
            Owner.LookRight = Owner.Horizontal > 0;

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
            EnterIdle();
    }
    #endregion

    #region Turn
    private void EnterTurn()
    {
        Owner.CreatureState = ECreatureState.Turn;
        PlayAnimation("Turn");
    }

    private void UpdateTurn()
    {
        if (IsAnimFinished() == false)
            return;

        // To RunMid
        if (HasInput && InputRight == Owner.LookRight)
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

}
