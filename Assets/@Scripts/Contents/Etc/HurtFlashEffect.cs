using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtFlashEffect : InitBase
{
    private int _flashCount = 1;
    private Color _flashColor = new Color(0.5f, 0, 0);
    private float _interval = 1.0f / 15;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;

        return true;
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_interval);

        for (int i = 0; i < _flashCount; i++)
        {
            _spriteRenderer.color = _flashColor;
            yield return wait;

            _spriteRenderer.color = _originalColor;
            yield return wait;
        }
    }
}
