using UnityEngine;
using static Define;

public class HitEffectBase : EffectBase
{
    protected ECreatureState lastState;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();

        lastState = Owner.CreatureState;
    }

    public override bool ClearEffect(EEffectClearType clearType)
    {
        if (base.ClearEffect(clearType) == true)
            Owner.CreatureState = lastState;

        return true;
    }
}
