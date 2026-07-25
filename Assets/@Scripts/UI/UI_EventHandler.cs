using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, ISubmitHandler, IPointerEnterHandler
{
    public event Action<PointerEventData> OnClickHandler = null;
    public event Action<PointerEventData> OnPointerDownHandler = null;
    public event Action<PointerEventData> OnPointerUpHandler = null;
    public event Action<PointerEventData> OnDragHandler = null;
    public event Action<PointerEventData> OnPointerEnterHandler = null;

    public event Action OnEnterHandler = null;
    public event Action OnAnyKeyHandler = null;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            OnAnyKeyHandler?.Invoke();
        }

        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame && EventSystem.current.currentSelectedGameObject == null)
        {
            if (EventSystem.current.firstSelectedGameObject != null)
            {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler?.Invoke(eventData);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(eventData);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnEnterHandler?.Invoke();
        OnClickHandler?.Invoke(new PointerEventData(EventSystem.current));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        OnPointerEnterHandler?.Invoke(eventData);
    }
}