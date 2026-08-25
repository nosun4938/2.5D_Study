using Data;
using UnityEngine;
using static Define;

public class QuestTask
{
    public QuestTaskData TaskData { get; private set; }
    public int Count { get; set; }

    public QuestTask(QuestTaskData questTaskData, int count)
    {
        TaskData = questTaskData;
        Count = count;
    }

    public bool IsCompleted()
    {
        if (TaskData.ObjectiveCount <= Count)
            return true;

        return false;
    }

    public void OnHandleBroadcastEvent(EBroadcastEventType eventType, int value)
    {
        switch (TaskData.ObjectiveType)
        {
            case EQuestObjectiveType.KillMonster:
                if (eventType == EBroadcastEventType.KillMonster)
                {
                    Count += value;
                }
                break;
            case EQuestObjectiveType.SpendMoney:
            case EQuestObjectiveType.EarnMoney:
                if (eventType == EBroadcastEventType.ChangeMoney)
                {
                    Count += value;
                }
                break;
            case EQuestObjectiveType.UseItem:
                break;
            case EQuestObjectiveType.Survival:
                break;
        }
    }
}
