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
    Dictionary<ESkillSlot, bool> _inputPressed = new();
    Dictionary<ESkillSlot, float> _lastInputTime = new();

    public bool HasJumped { get; set; } = false;
    public bool IsJumpPressed { get; set; } = false;
    
    public float CoyoteTimeCounter { get; set; }
    public float JumpBufferTimeCounter { get; set; }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Input System
        foreach (ESkillSlot slot in Enum.GetValues(typeof(ESkillSlot)))
        {
            _inputPressed[slot] = false;
        }

        // Layer
        gameObject.layer = LayerMask.NameToLayer("Player");
        CreatureType = ECreatureType.Player;

        return true;
    }

    public override void Update()
    {
        base.Update();
        HandleCoyoteTime();
        HandleBufferedInput();
    }

    public void FixedUpdate()
    {

    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        PlayerData = CreatureData as PlayerData;

        // State
        CreatureState = ECreatureState.Idle;

        // State Machine
        //_stateMachine.ChangeState(_idleState);
    }

    #region Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        Horizontal = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        BufferInput(ESkillSlot.Jump);
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
            //_stateMachine.ChangeState(_jumpState);
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
            HasJumped == false &&
            CreatureState != ECreatureState.Airborne;
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

        float currentSpeed = Rigidbody.linearVelocityX;

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

        Rigidbody.linearVelocityX = currentSpeed;
    }
    #endregion
}