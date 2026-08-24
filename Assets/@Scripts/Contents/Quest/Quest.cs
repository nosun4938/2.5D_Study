using Data;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Quest
{
    public QuestSaveData SaveData;
    private QuestData _questData;

    public List<QuestTask> _questTasks = new List<QuestTask>();

    public int TemplateID
    {
        get { return SaveData.TemplateID; }
        set { SaveData.TemplateID = value; }
    }

    public EQuestState State
    {
        get { return SaveData.State; }
        set { SaveData.State = value; }
    }

    public Quest(int templateID)
    {
        TemplateID = templateID;
        State = EQuestState.None;

        _questData = Managers.Data.QuestDic[templateID];
        _questTasks.Clear();

        foreach(QuestTaskData taskData in _questData.QuestTasks)
        {
            _questTasks.Add(new QuestTask(taskData));
        }
    }

    public bool IsCompleted()
    {
        for (int i = 0; i < _questData.QuestTasks.Count; i++)
        {
            if (i < SaveData.ProgressCount.Count)
                return false;

            QuestTaskData questTaskData = _questData.QuestTasks[i];

            int progressCount = SaveData.ProgressCount[i];
            if (progressCount < questTaskData.ObjectiveCount)
                return false;
        }
        return true;
    }

    public static Quest MakeQuest(QuestSaveData saveData)
    {
        if (Managers.Data.QuestDic.TryGetValue(saveData.TemplateID, out QuestData questData) == false)
            return null;

        Quest quest = null;

        quest = new Quest(saveData.TemplateID);

        if (quest != null)
        {
            quest.SaveData = saveData;
        }

        return quest;
    }

    public void OnHandleBroadcastEvent(EBroadcastEventType eventType, int value)
    {
        switch (eventType)
        {
            case EBroadcastEventType.ChangeMoney:
                break;
            case EBroadcastEventType.KillMonster:
                break;
            case EBroadcastEventType.LevelUp:
                break;
        }
    }
}
