using Data;
using UnityEngine;
using DG.Tweening;
using static Define;

public class ItemHolder : BaseObject
{
    public Data.ItemData ItemData { get; private set; }
    private ParabolaMotion _parabolaMotion;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.ItemHolder;
        _parabolaMotion = gameObject.GetOrAddComponent<ParabolaMotion>();

        return true;
    }

    public void SetInfo(int itemHolderID, int itemDataID, Vector3 pos)
    {
        ItemData = Managers.Data.ItemDic[itemDataID];
        SpriteRenderer.sprite = Managers.Resource.Load<Sprite>("Object_Coin.sprite");
        _parabolaMotion.SetInfo(transform.position, pos, endCallback: Arrived);
    }

    void Arrived()
    {
        SpriteRenderer.DOFade(0, 1f).OnComplete(() =>
        {
            if (ItemData != null)
            {

            }

            Managers.Object.Despawn(this);
        });
    }
}
