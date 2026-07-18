using Data;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    #region Components & Enums
    public BoxCollider2D Collider { get; private set; }
    public Data.CreatureData CreatureData { get; private set; }

    public BoxCollider2D HitCircle {  get; private set; }
    public Transform GroundCheck { get; protected set; }
    public Transform WallCheck { get; protected set; }
    public Transform HitCheck { get; private set; }

    public ECreatureType CreatureType { get; protected set; } = ECreatureType.None;

    protected ECreatureState _creatureState = ECreatureState.None;
    public virtual ECreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            if (_creatureState != value)
            {
                _creatureState = value;
            }
        }
    }

    public float Horizontal { get; set; }
    public bool IsGrounded { get; set; } = false;
    public Vector2 LastPosition { get; set; }

    public bool CheckIsGrounded()
    {
        //return Physics2D.OverlapCircle(WallCheck.position, 0.55f, LayerMask.GetMask("Ground"));
        return Physics2D.OverlapBox(GroundCheck.position, new Vector2(CreatureData.HitBox.Size.x - 0.1f, 0.5f), 0f, LayerMask.GetMask("Ground"));
    }

    public bool CheckIsBounded()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.55f, LayerMask.GetMask("Wall"));
    }

    private void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        if (GroundCheck == null)
            return;

        Handles.color = new Color(1, 0, 0, 0.4f);
        Handles.DrawWireCube(GroundCheck.position, new Vector2(CreatureData.HitBox.Size.x, 0.5f));
    #endif
    }
    #endregion

    #region Stats
    // Creature
    public float Hp { get; protected set; }
    public float MaxHp {  get; protected set; }
    public float MoveSpeed { get; set; }
    public float JumpSpeed { get; set; }
    public float Acceleration { get; set; }
    public float Deceleration { get; set; }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Creature;

        // Physics Checker
        GroundCheck = transform.Find("@groundCheck");
        if (GroundCheck == null)
        {
            GameObject obj = new GameObject("@groundCheck");
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            GroundCheck = obj.transform;
        }

        WallCheck = transform.Find("@wallCheck");
        if (WallCheck == null)
        {
            GameObject obj = new GameObject("@wallCheck");
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            WallCheck = obj.transform;
        }

        // Collider 이름을 데이터 상 HitBox로 만들어버려서, 피격 판정 이름을 HitCircle로 지음
        // 그래서 이름은 Circle인데 형태는 Box임 (개발 초기에는 형태도 Circle이었음)
        HitCheck = transform.Find("@hitCircle");
        if (HitCheck == null)
        {
            GameObject obj = new GameObject("@hitCheck");
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0.25f, 0);
            obj.layer = LayerMask.NameToLayer("HitCircle");
            HitCheck = obj.transform;
        }

        return true;
    }

    public virtual void SetInfo(int templateID)
    {
        DataTemplateID = templateID;

        if (CreatureType == ECreatureType.Player)
            CreatureData = Managers.Data.PlayerDic[templateID];

        gameObject.name = $"{CreatureData.DataID}_{CreatureData.DescriptionTextID}";

        // Collider
        Collider = gameObject.GetOrAddComponent<BoxCollider2D>();
        Collider.offset = CreatureData.HitBox.Offset;
        Collider.size = CreatureData.HitBox.Size;

        HitCircle = HitCheck.gameObject.GetOrAddComponent<BoxCollider2D>();
        HitCircle.offset = CreatureData.HitCircle.Offset;
        HitCircle.size = CreatureData.HitCircle.Size;
        HitCircle.isTrigger = true;
        
        // RigidBody
        Rigidbody.mass = CreatureData.Mass;
        Rigidbody.gravityScale = 5.0f;

        // Animator
        Animator animator = GetComponent<Animator>();
        if (animator == null)
            animator = gameObject.GetOrAddComponent<Animator>();

        animator.runtimeAnimatorController = Managers.Resource.Load<RuntimeAnimatorController>(CreatureData.AnimatorName);
        
        // Sprite Renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();

        // Player로 옮기자.
        Creature creature = gameObject.GetComponent<Creature>();
        switch (creature.CreatureType)
        {
            case ECreatureType.Player:
                spriteRenderer.sortingOrder = SortingLayers.HERO;
                break;
            case ECreatureType.Monster:
                spriteRenderer.sortingOrder = SortingLayers.MONSTER;
                break;
            case ECreatureType.Boss:
                spriteRenderer.sortingOrder = SortingLayers.BOSS;
                break;
            case ECreatureType.Artifact:
                spriteRenderer.sortingOrder= SortingLayers.ARTIFACT;
                break;
        }

        // Stat
        MaxHp = CreatureData.MaxHp;
        Hp = CreatureData.MaxHp;
        MoveSpeed = CreatureData.MoveSpeed;
        JumpSpeed = CreatureData.JumpSpeed;
        Acceleration = CreatureData.Acceleration;
        Deceleration = CreatureData.Deceleration;

        // State
        CreatureState = ECreatureState.Idle;
    }
    public virtual void Update()
    {
        IsGrounded = CheckIsGrounded();

        if (IsGrounded)
            LastPosition = transform.position;
    }
}
