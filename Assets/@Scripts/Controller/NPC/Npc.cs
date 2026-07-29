using Data;
using UnityEngine;
using static Define;

public class Npc : BaseObject
{
    public NpcData NpcData { get; set; }
    public BoxCollider InteractionBox { get; private set; }
    //private UI_NpcInteraction _ui;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Npc;

        return true;
    }

    public void SetInfo(int dataId)
    {
        NpcData = Managers.Data.NpcDic[dataId];
        gameObject.name = $"{NpcData.DataID}_{NpcData.DescriptionTextID}";

        Rigidbody.isKinematic = true;

        // Animator
        Animator animator = GetComponent<Animator>();
        if (animator == null)
            animator = gameObject.GetOrAddComponent<Animator>();

        animator.runtimeAnimatorController = Managers.Resource.Load<RuntimeAnimatorController>(NpcData.AnimatorName);
        animator.Play("Idle", 0, 0);

        // Sprite Renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = SortingLayers.NPC;

        // Interaction Box
        InteractionBox = gameObject.GetComponent<BoxCollider>();
        InteractionBox.center = NpcData.InteractionBox.Offset;
        InteractionBox.size = NpcData.InteractionBox.Size;
        InteractionBox.isTrigger = true;

        // UI

        
    }
}
