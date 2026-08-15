using Data;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static Define;
using static UnityEngine.UI.GridLayoutGroup;

public class Player : Creature
{
    public Data.HeroData HeroData { get; private set; }

    #region Variables
    public bool HasJumped { get; set; } = false;
    public bool IsJumpPressed { get; set; } = false;

    public float CoyoteTimeCounter { get; set; }
    public float JumpBufferTimeCounter { get; set; }
    #endregion

    #region StateMachine
    PlayerStateMachine _stateMachine;
    public EStateChangeReason ChangeReason { get; set; }

    // Movements
    public Player_Ground _groundState { get; private set; }
    public Player_Air _airState { get; private set; }
    public Player_GroundSkill _groundSkillState { get; private set; }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Layer
        gameObject.layer = LayerMask.NameToLayer("Player");
        ObjectType = EObjectType.Player;

        // StateMachine
        _stateMachine = new PlayerStateMachine(this);
        _groundState = new(this, _stateMachine);
        _airState = new(this, _stateMachine);
        _groundSkillState = new(this, _stateMachine);

        return true;
    }

    public override void Update()
    {
        base.Update();
        HandleCoyoteTime();
        HandleBufferedInput();

        _stateMachine?.Update();

        //Managers.Map.StageTransition.CheckMapChanged(transform.position);
    }

    public void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        HeroData = CreatureData as HeroData;

        // State Machine
        _stateMachine.ChangeState(_groundState);
    }

    #region Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        Horizontal = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsJumpPressed = true;
            BufferInput(ESkillSlot.Jump);
        }
        else if (context.canceled)
        {
            IsJumpPressed = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        BufferInput(ESkillSlot.Dash);
    }

    public void OnNormalAtk(InputAction.CallbackContext context)
    {
        BufferInput(ESkillSlot.NormalAtk);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }
    #endregion

    #region Input Buffering
    public float _lastSkillTime { get; set; }
    public struct BufferedInput
    {
        public ESkillSlot Slot;
        public float Time;
    }

    private List<BufferedInput> _inputBuffer = new List<BufferedInput>();
    private float _inputBufferTime = 0.2f;

    public void BufferInput(ESkillSlot slot)
    {
        _inputBuffer.Add(new BufferedInput
        {
            Slot = slot,
            Time = Time.time
        });
    }
    public void HandleBufferedInput()
    {
        if (TryConsumeBufferInput(CanUse, out ESkillSlot slot) == false)
            return;

        if (slot == ESkillSlot.Jump)
        {
            ChangeReason = EStateChangeReason.Jump;
            _stateMachine.ChangeState(_airState);
            return;
        }

        if (slot == ESkillSlot.Dash)
        {
            //_stateMachine.ChangeState(_dashState);
            return;
        }

        if (slot == ESkillSlot.NormalAtk)
        {
            ChangeReason = EStateChangeReason.NormalAtk;
            _stateMachine.ChangeState(_groundSkillState);
        }
    }
    public bool TryConsumeBufferInput(Func<ESkillSlot, bool> canUse, out ESkillSlot slot)
    {
        slot = ESkillSlot.None;

        for (int i = 0; i < _inputBuffer.Count; i++)
        {
            var input = _inputBuffer[i];

            if (Time.time - input.Time > _inputBufferTime)
            {
                _inputBuffer.RemoveAt(i);
                i--;
                continue;
            }

            if (canUse(input.Slot) == false)
                continue;

            slot = input.Slot;
            _inputBuffer.RemoveAt(i);
            Debug.Log($"{slot} is Used");

            return true;
        }

        return false;
    }

    public bool CanUse(ESkillSlot slot)
    {
        if (slot == ESkillSlot.Jump)
            return CanJump();
        if (slot == ESkillSlot.Dash)
            return false;
        if (slot == ESkillSlot.NormalAtk)
            return (CanJump() && CreatureState != ECreatureState.Skill);

        return true;
    }

    public bool CanJump()
    {
        return CoyoteTimeCounter > 0f &&
            HasJumped == false;
    }

    public void HandleCoyoteTime()
    {
        if (IsGrounded)
        {
            CoyoteTimeCounter = 0.5f;
            HasJumped = false;
        }
        else
        {
            CoyoteTimeCounter -= Time.deltaTime;
        }
    }
    #endregion
}