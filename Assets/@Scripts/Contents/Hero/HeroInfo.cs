using Data;
using UnityEngine;
using static Define;

public class HeroInfo
{
    public HeroSaveData SaveData { get; set; }

    public int TemplateID
    {
        get { return SaveData.DataID; }
        set { SaveData.DataID = value; }
    }

    public int Level
    {
        get { return SaveData.Level; }
        set { SaveData.Level = value; }
    }

    public int Exp
    {
        get { return SaveData.Exp; }
        set { SaveData.Exp = value; }
    }

    public EHeroOwningState OwningState
    {
        get { return SaveData.OwningState; }
        set { SaveData.OwningState = value; }
    }

    public HeroData HeroData { get; private set; }

    public int ASkillDataId { get { return HeroData.SkillAID; } }
    public int BSkillDataId { get { return HeroData.SkillBID; } }

    public bool IsPicked()
    {
        return OwningState == EHeroOwningState.Picked;
    }

    public HeroInfo(HeroSaveData saveData)
    {
        SaveData = saveData;

        if (Managers.Data.HeroDic.TryGetValue(saveData.DataID, out HeroData data))
            HeroData = data;

        OwningState = saveData.OwningState;
    }

    public static HeroInfo MakeHeroInfo(HeroSaveData saveData)
    {
        HeroInfo heroInfo = new HeroInfo(saveData);
        return heroInfo;
    }
}
