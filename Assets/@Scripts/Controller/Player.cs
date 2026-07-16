using Data;
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
        //Debug.LogWarning($"Hero.Init() »£√‚µ \n{Environment.StackTrace}");
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
        HandleBufferedInput();
        HandleHoldInput();
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
    public float _lastSkillTime { get; set; }
    float _repeatDelay = 0.3f;

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

    public void HandleHoldInput()
    {
        foreach (var pair in _inputPressed)
        {
            ESkillSlot slot = pair.Key;
            bool isPressed = pair.Value;

            if (!isPressed)
                continue;

            //if (!Owner._lastInputTime.TryGetValue(slot, out float lastTime))
            //continue;

            if (Time.time - _lastInputTime[slot] < _repeatDelay)
                continue;
            _lastInputTime[slot] = Time.time;

            BufferInput(slot);
        }
    }

    public void HandleBufferedInput()
    {
        if (TryConsumeBufferInput(CanUseSkill, out ESkillSlot slot) == false)
            return;

        if (slot == ESkillSlot.C)
        {
            
        }

        if (slot == ESkillSlot.F)
        {
            
        }
    }

    public bool CanJump()
    {
        return CoyoteTimeCounter > 0f &&
            HasJumped == false &&
            CreatureState != ECreatureState.Airborne;
    }
    public bool CanUseSkill(ESkillSlot slot)
    {
        return true;
    }

    /*
    public void OnMove(InputAction.CallbackContext context) => _stateMachine?.OnMove(context);
    public void OnJump(InputAction.CallbackContext context) => _stateMachine?.OnJump(context);
    public void OnDash(InputAction.CallbackContext context) => _stateMachine?.OnDash(context);

    // Skill
    public void OnZSkill(InputAction.CallbackContext context) => _stateMachine?.OnZSkill(context);
    public void OnXSkill(InputAction.CallbackContext context) => _stateMachine?.OnXSkill(context);
    public void OnASkill(InputAction.CallbackContext context) => _stateMachine?.OnASkill(context);
    public void OnSSkill(InputAction.CallbackContext context) => _stateMachine?.OnSSkill(context);
    public void OnDSkill(InputAction.CallbackContext context) => _stateMachine?.OnDSkill(context);
    public void OnFSkill(InputAction.CallbackContext context) => _stateMachine?.OnFSkill(context);
    */
    #endregion
}