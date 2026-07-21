using Data;
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
    #region Data
    public Data.PlayerData PlayerData { get; private set; }
    #endregion
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
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Layer
        gameObject.layer = LayerMask.NameToLayer("Player");
        CreatureType = ECreatureType.Player;

        // StateMachine
        _stateMachine = new PlayerStateMachine(this);
        _groundState = new(this, _stateMachine);
        _airState = new(this, _stateMachine);

        return true;
    }

    public override void Update()
    {
        base.Update();
        HandleCoyoteTime();
        HandleBufferedInput();

        _stateMachine?.Update();
    }

    public void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        PlayerData = CreatureData as PlayerData;

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
    #endregion

    #region Input Buffering
    public float _lastSkillTime { get; set; }
    public struct BufferedInput
    {
        public ESkillSlot Slot;
        public float Time;
    }

    private List<BufferedInput> _inputBuffer = new List<BufferedInput>();
    private float _inputBufferTime = 1.0f;

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
            //Debug.Log($"{slot} is Used");

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

    #region Move
    public void HorizontalMove()
    {
        float targetSpeed = Horizontal * MoveSpeed;

        float accel = IsGrounded
            ? Acceleration
            : Acceleration * 0.8f;

        float decel = IsGrounded
            ? Deceleration
            : Deceleration * 0.8f;

        float currentSpeed = Rigidbody.linearVelocity.x;

        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                targetSpeed,
                accel * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                decel * Time.fixedDeltaTime);
        }

        Rigidbody.SetVelocityX(currentSpeed);
    }

    public void LookDirection()
    {
        if (Horizontal != 0)
            LookRight = Horizontal > 0;
    }
    #endregion
}