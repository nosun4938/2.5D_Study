using Data;
using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Quest
{
    public QuestSaveData SaveData { get; set; }
    private QuestData QuestData { get; set; }
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

    public QuestTask GetCurrentTask()
    {
        foreach (QuestTask task in _questTasks)
        {
            if (task.IsCompleted() == false)
                return task;
        }

        return null;
    }

    public bool IsCompleted()
    {
        for (int i = 0; i < QuestData.QuestTasks.Count; i++)
        {
            if (i >= SaveData.ProgressCount.Count)
                return false;

            QuestTaskData questTaskData = QuestData.QuestTasks[i];

            int progressCount = SaveData.ProgressCount[i];
            if (progressCount < questTaskData.ObjectiveCount)
                return false;
        }

        return true;
    }

    public Quest(QuestSaveData saveData)
    {
        SaveData = saveData;
        State = EQuestState.None;
        QuestData = Managers.Data.QuestDic[TemplateID];

        _questTasks.Clear();

        for (int i = 0; i < QuestData.QuestTasks.Count; i++)
        {
            _questTasks.Add(new QuestTask(QuestData.QuestTasks[i], saveData.ProgressCount[i]));
        }
    }

    public void GiveReward()
    {
        if (SaveData.State == EQuestState.Rewarded)
            return;

        if (IsCompleted() == false)
            return;

        SaveData.State = EQuestState.Rewarded;

        foreach (var reward in QuestData.Rewards)
        {
            switch (reward.RewardType)
            {
                case EQuestRewardType.Colleague:
                    int heroID = reward.RewardDataID;
                    Managers.Hero.AcquireHeroCard(heroID, reward.RewardCount);
                    Managers.Hero.PickHero(heroID, Vector3.zero);
                    break;
                case EQuestRewardType.SkillBook:
                    
                    break;
                case EQuestRewardType.Money:
                    Debug.Log($"Get Money");
                    break;
            }
        }
    }

    public static Quest MakeQuest(QuestSaveData saveData)
    {
        if (Managers.Data.QuestDic.TryGetValue(saveData.TemplateID, out QuestData questData) == false)
            return null;

        Quest quest = new Quest(saveData);
        return quest;
    }

    public void OnHandleBroadcastEvent(EBroadcastEventType eventType, int value)
    {
        if (eventType == EBroadcastEventType.QuestClear)
            return;

        GetCurrentTask().OnHandleBroadcastEvent(eventType, value);

        for (int i = 0; i < _questTasks.Count; i++)
        {
            SaveData.ProgressCount[i] = _questTasks[i].Count;
        }

        if (IsCompleted() && State != EQuestState.Rewarded)
        {
            State = EQuestState.Completed;
            GiveReward(); // Rewarded State
            Managers.Game.BroadcastEvent(EBroadcastEventType.QuestClear, QuestData.DataID);
        }
    }
}
