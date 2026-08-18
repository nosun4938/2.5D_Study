using System.Collections;
using UnityEngine;
using static Define;

public class Hitstun : HitEffectBase
{
    [SerializeField]
    private float _hitstunDistance = 5f;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        StopCoroutine(DoHitstun(lastState));
        StartCoroutine(DoHitstun(lastState));
    }

    IEnumerator DoHitstun(ECreatureState lastState)
    {
        float dir = Owner.LookRight ? 1 : -1;
        Vector3 originalPosition = Owner.transform.position;
        Vector3 horizonPosition = originalPosition - dir * _hitstunDistance * Vector3.right;

        float TickTime = 0.5f;
        for (float t = 0; t < TickTime; t += Time.deltaTime)
        {
            float normalizedTime = t / TickTime;
            Owner.transform.position = Vector3.Lerp(originalPosition, horizonPosition, normalizedTime);
            yield return null;
        }

        ClearEffect(EEffectClearType.End);
    }
}
