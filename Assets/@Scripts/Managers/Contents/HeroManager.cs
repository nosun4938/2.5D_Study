using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class HeroManager
{
    public Dictionary<int, HeroInfo> AllHeroInfos { get; set; } = new Dictionary<int, HeroInfo>();

    public List<HeroInfo> PickedHeroes
    {
        get { return AllHeroInfos.Values.Where(h => h.OwningState == EHeroOwningState.Picked).ToList(); }
    }
    public List<HeroInfo> OwnedHeroes
    {
        get { return AllHeroInfos.Values.Where(h => h.OwningState == EHeroOwningState.Owned).ToList(); }
    }
    public List<HeroInfo> UnownedHeroes
    {
        get { return AllHeroInfos.Values.Where(h => h.OwningState == EHeroOwningState.Unowned).ToList(); }
    }

    public HeroInfo GetHeroInfo(int templateId)
    {
        AllHeroInfos.TryGetValue(templateId, out HeroInfo heroInfo);
        return heroInfo;
    }

    public HeroSaveData MakeHeroInfo(int templateId)
    {
        if (Managers.Data.HeroDic.TryGetValue(templateId, out HeroData heroInfoData) == false)
            return null;

        HeroSaveData saveData = new HeroSaveData()
        {
            DataID = heroInfoData.DataID,
            Level = 1,
            Exp = 0,
            OwningState = EHeroOwningState.Unowned
        };

        AddHeroInfo(saveData);
        return saveData;
    }

    public HeroInfo AddHeroInfo(HeroSaveData saveData)
    {
        HeroInfo heroInfo = HeroInfo.MakeHeroInfo(saveData);
        if (heroInfo == null)
            return null;

        AllHeroInfos.Add(heroInfo.TemplateID, heroInfo);
        return heroInfo;
    }

    public Hero PickHero(int templateID, Vector3 pos)
    {
        HeroInfo heroInfo = GetHeroInfo(templateID);
        if (heroInfo == null)
        {
            Debug.Log("영웅존재안함");
            return null;
        }

        heroInfo.OwningState = EHeroOwningState.Picked;

        Hero hero = Managers.Object.Spawn<Hero>(pos, templateID);
        Managers.Game.BroadcastEvent(EBroadcastEventType.ChangeCrew, 0);

        return hero;
    }

    public void UnpickHero(int heroID)
    {
        if (AllHeroInfos.TryGetValue(heroID, out HeroInfo info) == false)
            return;

        if (info.OwningState == EHeroOwningState.Picked)
        {
            info.OwningState = EHeroOwningState.Owned;

            Hero hero = Managers.Object.Player;
            Managers.Object.Despawn(hero);

            Managers.Game.BroadcastEvent(EBroadcastEventType.ChangeCrew, heroID);
        }
    }

    public void AcquireHeroCard(int heroId, int exp)
    {
        if (AllHeroInfos.TryGetValue(heroId, out HeroInfo heroInfo) == false)
            return;

        if (heroInfo.OwningState == EHeroOwningState.Unowned)
        {
            heroInfo.OwningState = EHeroOwningState.Owned;
            heroInfo.Exp += exp;
        }
        else
        {
            heroInfo.Exp += exp;
        }
    }

    public void AddUnknownHeroes()
    {
        foreach (HeroData hero in Managers.Data.HeroDic.Values.ToList())
        {
            if (AllHeroInfos.ContainsKey(hero.DataID))
                continue;

            MakeHeroInfo(hero.DataID);
        }
    }
}
