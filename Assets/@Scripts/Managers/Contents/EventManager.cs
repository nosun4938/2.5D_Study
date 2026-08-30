using System;
using System.Collections.Generic;
using static Define;

public class EventManager
{
    private Dictionary<EBroadcastEventType, Action> _event = new Dictionary<EBroadcastEventType, Action>();

    public void Init()
    {
        _event.Clear();
    }

    public void AddEvent(EBroadcastEventType eventType, Action listener)
    {
        Action thisEvent;
        if (_event.TryGetValue(eventType, out thisEvent))
        {
            thisEvent += listener;
            _event[eventType] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            _event.Add(eventType, thisEvent);
        }
    }

    public void RemoveEvent(EBroadcastEventType eventType, Action listener)
    {
        if (_event == null)
        {
            return;
        }

        Action thisEvent;
        if (_event.TryGetValue(eventType, out thisEvent))
        {
            thisEvent -= listener;
            _event[eventType] = thisEvent;
        }
    }

    public void TriggerEvent(EBroadcastEventType eventType)
    {
        Action thisEvent;
        if (_event.TryGetValue(eventType, out thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

    public void Clear()
    {
        _event.Clear();
    }
}
