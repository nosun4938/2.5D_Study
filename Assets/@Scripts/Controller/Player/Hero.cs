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

public class Hero : Creature
{
    public Data.HeroData HeroData { get; private set; }

    public PlayerInput PlayerInput { get; private set; }
    InputActionMap playerMap;

    #region Variables
    public bool HasJumped { get; set; } = false;
    public bool IsJumpPressed { get; set; } = false;

    public float CoyoteTimeCounter { get; set; }
    public float JumpBufferTimeCounter { get; set; }
    public Npc NearbyNpc { get; set; }
    #endregion

    #region StateMachine
    HeroStateMachine _stateMachine;
    public EStateChangeReason ChangeReason { get; set; }

    // Movements
    public Hero_Ground _groundState { get; private set; }
    public Hero_Air _airState { get; private set; }
    public Hero_GroundSkill _groundSkillState { get; private set; }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Layer
        gameObject.layer = LayerMask.NameToLayer("Player");
        ObjectType = EObjectType.Hero;

        // InputSystem
        PlayerInput = gameObject.GetComponent<PlayerInput>();
        playerMap = PlayerInput.actions.FindActionMap("Player");

        // StateMachine
        _stateMachine = new HeroStateMachine(this);
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
            BufferInput(EKeySlot.Jump);
        }
        else if (context.canceled)
        {
            IsJumpPressed = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        BufferInput(EKeySlot.Dash);
    }

    public void OnNormalAtk(InputAction.CallbackContext context)
    {
        BufferInput(EKeySlot.NormalAtk);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BufferInput(EKeySlot.Interact);
        }
    }
    #endregion

    #region Input Buffering
    public float _lastSkillTime { get; set; }
    public struct BufferedInput
    {
        public EKeySlot Slot;
        public float Time;
    }

    private List<BufferedInput> _inputBuffer = new List<BufferedInput>();
    private float _inputBufferTime = 0.2f;

    public void BufferInput(EKeySlot slot)
    {
        _inputBuffer.Add(new BufferedInput
        {
            Slot = slot,
            Time = Time.time
        });
    }
    public void HandleBufferedInput()
    {
        if (TryConsumeBufferInput(CanUse, out EKeySlot slot) == false)
            return;

        if (slot == EKeySlot.Jump)
        {
            HasJumped = true;
            ChangeReason = EStateChangeReason.Jump;
            _stateMachine.ChangeState(_airState);
            return;
        }

        if (slot == EKeySlot.Dash)
        {
            //_stateMachine.ChangeState(_dashState);
            return;
        }

        if (slot == EKeySlot.NormalAtk)
        {
            ChangeReason = EStateChangeReason.NormalAtk;
            _stateMachine.ChangeState(_groundSkillState);
        }

        if (slot == EKeySlot.Interact)
        {
            if (NearbyNpc == null)
                return;
            if (NearbyNpc.Interaction == null)
                return;
            if (NearbyNpc.Interaction.CanInteract() == false)
                return;
            
            NearbyNpc.OnClickEvent();
        }
    }
    public bool TryConsumeBufferInput(Func<EKeySlot, bool> canUse, out EKeySlot slot)
    {
        slot = EKeySlot.None;

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

    public bool CanUse(EKeySlot slot)
    {
        switch (slot)
        {
            case EKeySlot.Jump:
                return CanJump();
            case EKeySlot.NormalAtk:
            case EKeySlot.Interact:
                return CanGroundAction();
            default:
                return true;
        }
    }

    public bool CanGroundAction()
    {
        return (IsGrounded
            && CreatureState != ECreatureState.Skill
            && CreatureState != ECreatureState.Hitstun);
    }
    public bool CanJump()
    {
        return (CoyoteTimeCounter > 0f
            && HasJumped == false
            && CreatureState != ECreatureState.Skill
            && CreatureState != ECreatureState.Hitstun);
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
    
    #region Input Lock 
    public void PlayerInputLock()
    {
        playerMap.Disable();
    }
    public void PlayerInputUnlock()
    {
        playerMap.Disable();
    }
    #endregion
}