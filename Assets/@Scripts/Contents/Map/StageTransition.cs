using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StageTransition : InitBase
{
    public List<Stage> Stages = new List<Stage>();

    public void SetInfo()
    {
        int currentMapIndex = 0;
        for (int i = 0; i < Stages.Count; i++)
        {
            Stages[i].SetInfo(i);

            // TODO
            // currentMapIndex가 0이 아닌경우
        }

        OnMapChanged(currentMapIndex);
    }

    public void OnMapChanged(int newMapIndex)
    {
        Managers.Game.CurrentStage = Stages[newMapIndex];
        Managers.Game.CurrentStageIndex = newMapIndex;

        LoadMapsAround(newMapIndex);
        UnloadOtherMaps(newMapIndex);
    }

    private void LoadMapsAround(int mapIndex)
    {
        // 이전, 현재, 다음 맵을 로드
        for (int i = mapIndex - 1; i <= mapIndex + 1; i++)
        {
            if (i > -1 && i < Stages.Count)
            {
                Debug.Log($"{i} Stage Load -> {Stages[i].name}");
                Stages[i].LoadStage();
            }
        }
    }

    private void UnloadOtherMaps(int mapIndex)
    {
        for (int i = 0; i < Stages.Count; i++)
        {
            if (i < mapIndex - 1 || i > mapIndex + 1)
            {
                Debug.Log($"{i} Stage UnLoad -> {Stages[i].name}");
                Stages[i].UnLoadStage();
            }
        }
    }
}
