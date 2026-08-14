using Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillComponent : InitBase
{
    public List<SkillBase> SkillList { get; } = new List<SkillBase>();
    public List<SkillBase> ActiveSkills { get; set; } = new List<SkillBase>();

    public SkillBase NormalAtk { get; private set; }

    Creature _owner;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(Creature owner, CreatureData creatureData)
    {
        _owner = owner;

        AddSkill(creatureData.NormalAtkID, ESkillSlot.NormalAtk);
    }

    public void AddSkill(int skillTemplateID, Define.ESkillSlot skillSlot)
    {
        if (skillTemplateID == 0)
            return;

        if (Managers.Data.SkillDic.TryGetValue(skillTemplateID, out var data) == false)
        {
            Debug.LogWarning($"AddSkill Failed {skillTemplateID}");
            return;
        }

        SkillBase skill = gameObject.AddComponent(Type.GetType(data.ClassName)) as SkillBase;
        if (skill == null)
            return;

        skill.SetInfo(_owner, skillTemplateID);

        SkillList.Add(skill);

        switch (skillSlot)
        {
            case Define.ESkillSlot.NormalAtk:
                NormalAtk = skill;
                break;
        }
    }
}
