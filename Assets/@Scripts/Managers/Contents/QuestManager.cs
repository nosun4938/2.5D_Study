using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class QuestManager
{
    public Dictionary<int, Quest> AllQuests = new Dictionary<int, Quest>();

    public void Init()
    {
        Managers.Game.OnBroadcastEvent -= OnHandleBroadcastEvent;
        Managers.Game.OnBroadcastEvent += OnHandleBroadcastEvent;
    }

    public void AddUnknownQuests()
    {
        foreach (QuestData questData in Managers.Data.QuestDic.Values.ToList())
        {
            if (AllQuests.ContainsKey(questData.DataID))
                continue;

            QuestSaveData questSaveData = new QuestSaveData()
            {
                TemplateID = questData.DataID,
                State = Define.EQuestState.None,
                NextResetTime = DateTime.MaxValue,
            };

            for (int i = 0; i < questData.QuestTasks.Count; i++)
                questSaveData.ProgressCount.Add(0);

            AddQuest(questSaveData);
        }
    }

    public void CheckWaitingQuests()
    {
        // TODO
    }

    public void CheckProcessingQuests()
    {
        // TODO
    }

    public Quest AddQuest(QuestSaveData questInfo)
    {
        Quest quest = Quest.MakeQuest(questInfo);
        if (quest == null)
            return null;

        AllQuests.Add(quest.TemplateID, quest);

        return quest;
    }

    public void Clear()
    {
        AllQuests.Clear();
    }

    void OnHandleBroadcastEvent(EBroadcastEventType eventType, int value)
    {
        foreach(Quest quest in AllQuests.Values)
        {
            if (quest.State == EQuestState.Processing)
                quest.OnHandleBroadcastEvent(eventType, value);
        }
    }
}
