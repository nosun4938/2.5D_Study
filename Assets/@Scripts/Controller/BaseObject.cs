using System.Buffers;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class BaseObject : InitBase
{
    public EObjectType ObjectType { get; protected set; } = EObjectType.None;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public Vector3 CenterPosition { get { return transform.position; } }
    
    public int DataTemplateID { get; set; }

    bool _lookRight = true;
    public bool LookRight
    {
        get { return _lookRight; }
        set
        {
            _lookRight = value;
            Flip(!value);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Rigidbody = gameObject.GetOrAddComponent<Rigidbody2D>();
        return true;
    }

    #region Animation

    public void Flip(bool flag)
    {
        if (SpriteRenderer == null)
            return;

        SpriteRenderer.flipX = flag;
    }

    #endregion
}
