using Data;
using System;
using System.Collections;
using UnityEngine;
using static Define;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Creature : BaseObject
{
    public Data.CreatureData CreatureData { get; private set; }
    #region Stats
    public float Hp { get; protected set; }
    public float MaxHp { get; protected set; }
    public float MoveSpeed { get; set; }
    public float JumpSpeed { get; set; }
    public float Acceleration { get; set; }
    public float Deceleration { get; set; }
    #endregion

    #region Components & Variables
    public BoxCollider Collider { get; private set; }
    public BoxCollider HitCircle {  get; private set; }
    public Transform GroundCheck { get; protected set; }
    public Transform WallCheck { get; protected set; }
    public Transform HitCheck { get; private set; }
    public SkillComponent Skills { get; protected set; }
    public SkillBase PlayingSkill { get; set; }

    protected ECreatureState _creatureState = ECreatureState.None;
    public virtual ECreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            if (_creatureState != value)
            {
                _creatureState = value;
                UpdateAnimation();
            }
        }
    }

    public float Horizontal { get; set; }
    public bool IsGrounded { get; set; } = false;
    public Vector2 LastPosition { get; set; }

    private Collider[] _overlapGrounds = new Collider[4];
    public bool CheckIsGrounded()
    {
        int hitCount = Physics.OverlapBoxNonAlloc(
            GroundCheck.position, 
            new Vector3(CreatureData.HitBox.Size.x - 0.1f, 0.5f, 0.5f), 
            _overlapGrounds, 
            Quaternion.identity, 
            LayerMask.GetMask("Ground"));

        return hitCount > 0;
    }

    private void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        if (GroundCheck == null)
            return;
        if (CreatureData == null)
            return;

        Handles.color = new Color(1, 0, 0, 0.4f);
        Handles.DrawWireCube(GroundCheck.position, new Vector3(CreatureData.HitBox.Size.x - 0.1f, 0.5f, 0.5f));
    #endif
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

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

        if (ObjectType == EObjectType.Player)
            CreatureData = Managers.Data.HeroDic[templateID];
        if (ObjectType == EObjectType.Monster)
            CreatureData = Managers.Data.MonsterDic[templateID];

        gameObject.name = $"{CreatureData.DataID}_{CreatureData.DescriptionTextID}";

        // Collider
        Collider = gameObject.GetOrAddComponent<BoxCollider>();
        Collider.center = CreatureData.HitBox.Offset;
        Collider.size = CreatureData.HitBox.Size;

        HitCircle = HitCheck.gameObject.GetOrAddComponent<BoxCollider>();
        HitCircle.center = CreatureData.HitCircle.Offset;
        HitCircle.size = CreatureData.HitCircle.Size;
        HitCircle.isTrigger = true;

        // Skills
        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        Skills.SetInfo(this, CreatureData);

        // RigidBody
        Rigidbody.mass = CreatureData.Mass;

        // Animator
        Animator animator = GetComponent<Animator>();
        if (animator == null)
            animator = gameObject.GetOrAddComponent<Animator>();

        animator.runtimeAnimatorController = Managers.Resource.Load<RuntimeAnimatorController>(CreatureData.AnimatorName);
        
        // Sprite Renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();

        // Sorting Layer
        switch (ObjectType)
        {
            case EObjectType.Player:
                spriteRenderer.sortingOrder = SortingLayers.HERO;
                break;
            case EObjectType.Monster:
                spriteRenderer.sortingOrder = SortingLayers.MONSTER;
                break;
            case EObjectType.Boss:
                spriteRenderer.sortingOrder = SortingLayers.BOSS;
                break;
        }

        // Stat
        MaxHp = CreatureData.MaxHp;
        Hp = CreatureData.MaxHp;
        MoveSpeed = CreatureData.MoveSpeed;
        JumpSpeed = CreatureData.JumpSpeed;
        Acceleration = CreatureData.Acceleration;
        Deceleration = CreatureData.Deceleration;
    }
    public virtual void Update()
    {
        IsGrounded = CheckIsGrounded();

        if (IsGrounded)
            LastPosition = transform.position;
    }

    #region Move
    public void HorizontalMove()
    {
        float targetSpeed = Horizontal * MoveSpeed;

        float accel = IsGrounded
            ? Acceleration
            : Acceleration * 0.8f;

        float decel = IsGrounded
            ? Deceleration
            : Deceleration * 0.8f;

        float currentSpeed = Rigidbody.linearVelocity.x;

        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                targetSpeed,
                accel * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                decel * Time.fixedDeltaTime);
        }

        Rigidbody.SetVelocityX(currentSpeed);
    }

    public void LookDirection()
    {
        if (Horizontal != 0)
            LookRight = Horizontal > 0;
    }
    #endregion

    #region Animation Helpers
    public string CurrentAnimName { get; set; }
    public void PlayAnimation(string animName)
    {
        if (CurrentAnimName == animName)
            return;

        CurrentAnimName = animName;
        Animator.Play(animName, 0, 0f);
    }

    public bool IsAnimFinished()
    {
        var info = Animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(CurrentAnimName) && info.normalizedTime >= 1f;
    }
    protected override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                PlayAnimation(AnimName.IDLE);
                break;
            case ECreatureState.RunStart:
                PlayAnimation(AnimName.RUNSTART);
                break;
            case ECreatureState.RunMid:
                PlayAnimation(AnimName.RUNMID);
                break;
            case ECreatureState.Stop:
                PlayAnimation(AnimName.STOP);
                break;
            case ECreatureState.Turn:
                PlayAnimation(AnimName.TURN);
                break;
            case ECreatureState.Jump:
                PlayAnimation(AnimName.JUMP);
                break;
            case ECreatureState.Fall:
                PlayAnimation(AnimName.FALL);
                break;
            case ECreatureState.Land:
                PlayAnimation(AnimName.LAND);
                break;

            default:
                break;
        }
    }
    #endregion
}
