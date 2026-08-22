using Data;
using UnityEngine;
using static Define;

public class Item
{
    public ItemSaveData SaveData { get; set; }

    public int InstanceID
    {
        get { return SaveData.InstanceID; }
        set { SaveData.InstanceID = value;}
    }
    public int DbID
    {
        get { return SaveData.DbID; }
    }
    public int TemplateID
    {
        get { return SaveData.TemplateID; }
        set { SaveData.TemplateID = value;}
    }
    public int Count
    {
        get { return SaveData.Count; }
        set { SaveData.Count = value;}
    }
    public int EquipSlot
    {
        get { return SaveData.EquipSlot; }
        set { SaveData.EquipSlot = value;}
    }

    public Data.ItemData TemplateData
    {
        get
        {
            return Managers.Data.ItemDic[TemplateID];
        }
    }

    public EItemType ItemType { get; private set; }
    public EItemSubType SubType { get; private set; }

    public Item(int templateID)
    {
        TemplateID = templateID;
        ItemType = TemplateData.ItemType;
        SubType = TemplateData.ItemSubType;
    }

    public virtual bool Init()
    {
        return true;
    }

    public static Item MakeItem(ItemSaveData itemInfo)
    {
        if (Managers.Data.ItemDic.TryGetValue(itemInfo.TemplateID, out ItemData itemData) == false)
            return null;

        Item item = null;

        switch (itemData.ItemType)
        {
            case EItemType.Sword:
                item = new Colleague(itemInfo.TemplateID);
                break;
            case EItemType.Claymore:
                item = new Colleague(itemInfo.TemplateID);
                break;
            case EItemType.Sword_A:
                item = new SkillBook(itemInfo.TemplateID);
                break;
            case EItemType.Sword_B:
                item = new SkillBook(itemInfo.TemplateID);
                break;
            case EItemType.Claymore_A:
                item = new SkillBook(itemInfo.TemplateID);
                break;
            case EItemType.Claymore_B:
                item = new SkillBook(itemInfo.TemplateID);
                break;
            case EItemType.Money:
                item = new Consumable(itemInfo.TemplateID);
                break;
            case EItemType.Potion:
                item = new Consumable(itemInfo.TemplateID);
                break;
        }

        if (item != null)
        {
            item.SaveData = itemInfo;
            item.InstanceID = itemInfo.InstanceID;
            item.Count = itemInfo.Count;
        }

        return item;
    }

    #region Helper
    public bool IsEquippable()
    {
        return GetEquipItemEquipSlot() != EEquipSlotType.None;
    }

    public EEquipSlotType GetEquipItemEquipSlot()
    {
        if (ItemType == EItemType.Sword)
            return EEquipSlotType.Weapon;
        if (ItemType == EItemType.Claymore)
            return EEquipSlotType.Weapon;

        if (ItemType == EItemType.Sword_A)
            return EEquipSlotType.Skill_A;
        if (ItemType == EItemType.Sword_B)
            return EEquipSlotType.Skill_B;

        if (ItemType == EItemType.Claymore_A)
            return EEquipSlotType.Skill_A;
        if (ItemType == EItemType.Claymore_B)
            return EEquipSlotType.Skill_B;

        return EEquipSlotType.None;
    }

    public bool IsEquippedItem()
    {
        return SaveData.EquipSlot > (int)EEquipSlotType.None && SaveData.EquipSlot < (int)EEquipSlotType.EquipMax;
    }

    public bool IsInInventory()
    {
        return SaveData.EquipSlot == (int)EEquipSlotType.Inventory;
    }
    #endregion
}

public class Colleague : Item
{
    protected Data.ColleagueData ColleagueData { get { return (Data.ColleagueData)TemplateData; } }

    public Colleague(int templateID) : base(templateID)
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (TemplateData == null)
            return false;

        if (TemplateData.ItemType != EItemType.Sword
            && TemplateData.ItemType != EItemType.Claymore)
            return false;

        ColleagueData data = (ColleagueData)TemplateData;
        {

        }

        return true;
    }
}

public class SkillBook : Item
{
    protected Data.SkillBookData SkillBookData { get { return (Data.SkillBookData)TemplateData; } }

    public SkillBook(int templateID) : base(templateID)
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (TemplateData == null)
            return false;

        if (TemplateData.ItemType != EItemType.Sword_A
            && TemplateData.ItemType != EItemType.Sword_B
            && TemplateData.ItemType != EItemType.Claymore_A
            && TemplateData.ItemType != EItemType.Claymore_B)
            return false;

        SkillBookData data = (SkillBookData)TemplateData;
        {

        }

        return true;
    }
}

public class Consumable : Item
{
    public int Value;

    protected Data.ConsumableData ConsumableData { get { return (Data.ConsumableData)TemplateData; } }

    public Consumable(int templateID) : base(templateID)
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (TemplateData == null)
            return false;

        if (TemplateData.ItemType != EItemType.Money
            && TemplateData.ItemType != EItemType.Potion)
            return false;

        ConsumableData data = (ConsumableData)TemplateData;
        {
            Value = data.Value;
        }

        return true;
    }
}
