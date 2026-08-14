using UnityEngine;

public class Hero_NormalAtk : SkillBase
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo(Creature owner, int skillTemplateID)
    {
        base.SetInfo(owner, skillTemplateID);
    }

    public override void DoSkill()
    {
        base.DoSkill();
        Owner.CreatureState = Define.ECreatureState.Skill;
    }

    protected override void OnAttackEvent()
    {
        
    }
}
