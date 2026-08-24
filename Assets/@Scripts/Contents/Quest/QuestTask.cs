using Data;
using UnityEngine;
using static Define;

public class QuestTask
{
    public QuestTaskData _questTaskData;
    public int Count { get; set; }

    public QuestTask(QuestTaskData questTaskData)
    {
        _questTaskData = questTaskData;
    }
    public bool IsCompleted()
    {
        return false;
    }

    public void OnHandleBroadcastEvent(EBroadcastEventType eventType, int value)
    {
        // _questTaskData.ObjectType 와 eventType을 비교해서 Count 변경
    }
}
