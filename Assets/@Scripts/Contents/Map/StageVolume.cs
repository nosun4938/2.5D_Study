using UnityEngine;

public class StageVolume : MonoBehaviour
{
    public Collider Volume { get; set; }
    public int StageIndex { get; set; } = -1;
    public void SetInfo(int stageIndex)
    {
        Volume = gameObject.GetComponent<Collider>();
        StageIndex = stageIndex;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") == false)
            return;

        Debug.Log($"Now Map Index: {StageIndex}");

        int currentIndex = Managers.Game.CurrentStageIndex;
        if (currentIndex != StageIndex)
            Managers.Map.StageTransition.OnMapChanged(StageIndex);
    }
}
