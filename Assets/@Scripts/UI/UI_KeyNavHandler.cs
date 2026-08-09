using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_KeyNavHandler : MonoBehaviour
{
    public event Action<int, GameObject> OnSelectionChanged;

    private List<GameObject> _items = new List<GameObject>();
    private Action _onCancel;

    private KeyCode _moveNextKey = KeyCode.DownArrow;
    private KeyCode _movePrevKey = KeyCode.UpArrow;
    private KeyCode _confirmKey = KeyCode.Return;
    private KeyCode _cancelKey = KeyCode.Escape;

    private int _index = 0;
    private bool _active = true;

    public int CurrentIndex => _index;
    public GameObject CurrentItem => (_items.Count > 0) ? _items[_index] : null;

    public void Setup(
        List<GameObject> items,
        Action onCancel,
        int startIndex = 0,
        KeyCode moveNextKey = KeyCode.DownArrow,
        KeyCode movePrevKey = KeyCode.UpArrow,
        KeyCode confirmKey = KeyCode.Return,
        KeyCode cancelKey = KeyCode.Escape)
    {
        _items = items ?? new List<GameObject>();
        _onCancel = onCancel;

        _moveNextKey = moveNextKey;
        _movePrevKey = movePrevKey;
        _confirmKey = confirmKey;
        _cancelKey = cancelKey;

        _index = (_items.Count == 0) ? 0 : Mathf.Clamp(startIndex, 0, _items.Count - 1);
        _active = true;

        NotifySelectionChanged();
    }
    public void SetActive(bool active) { _active = active; }

    private void Update()
    {
        if (_active == false)
            return;

        if (Input.GetKeyDown(_cancelKey))
        {
            _onCancel?.Invoke();
            return;
        }

        if (_items == null || _items.Count == 0)
            return;

        if (Input.GetKeyDown(_movePrevKey))
            Move(-1);
        else if (Input.GetKeyDown(_moveNextKey))
            Move(1);
        else if (Input.GetKeyDown(_confirmKey))
            Confirm();
    }

    private void Move(int dir)
    {
        _index = (_index + dir + _items.Count) % _items.Count;
        NotifySelectionChanged();
    }

    private void Confirm()
    {
        GameObject item = CurrentItem;
        if (item == null)
            return;

        UI_EventHandler evt = item.GetComponent<UI_EventHandler>();
        evt?.OnPointerClick(null);
    }

    private void NotifySelectionChanged()
    {
        OnSelectionChanged?.Invoke(_index, CurrentItem);
    }

}
