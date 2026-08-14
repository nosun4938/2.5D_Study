using Data;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public Data.MonsterData MonsterData { get; private set; }

    public override ECreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            if (_creatureState != value)
            {
                base.CreatureState = value;
                UpdateAnimation();
            }
        }
    }

    #region StateMachine
    MonsterStateMachine _stateMachine;
    public EStateChangeReason ChangeReason { get; set; }

    // Movements
    public Monster_Ground _groundState { get; private set; }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;

        // StateMachine
        _stateMachine = new MonsterStateMachine(this);
        _groundState = new(this, _stateMachine);

        return true;
    }

    public override void Update()
    {
        base.Update();

        _stateMachine?.Update();
    }

    public void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        MonsterData = CreatureData as MonsterData;

        // State Machine
        _stateMachine.ChangeState(_groundState);
    }
}
