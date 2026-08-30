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

    #region Battle
    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        LookRight = attacker.transform.position.x > transform.position.x;
        base.OnDamaged(attacker, skill);
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);

        // Item Drop
        ItemData rewardItem = Managers.Data.ItemDic[MonsterData.DropItemID];
        if (rewardItem != null)
        {
            ItemHolder itemHolder = Managers.Object.Spawn<ItemHolder>(transform.position, MonsterData.DropItemID);
            
            Vector3 ranLeft = new Vector3(transform.position.x + Random.Range(-10, -15) * 0.1f, transform.position.y + 10, transform.position.z);
            Vector3 ranRight = new Vector3(transform.position.x + Random.Range(10, 15) * 0.1f, transform.position.y + 10, transform.position.z);
            Vector3 dropPos = Random.value < 0.5 ? ranLeft : ranRight;

            itemHolder.SetInfo(0, MonsterData.DropItemID, dropPos);
        }

        // BroadCast
        Managers.Game.BroadcastEvent(EBroadcastEventType.KillMonster, MonsterData.DataID);

        Managers.Object.Despawn(this);
    }
    #endregion


}
