using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageData currentStage;

    int killCount;

    void OnEnable()
    {
        GameEvents.OnEnemyKilled += OnEnemyKilled;
    }

    void OnDisable()
    {
        GameEvents.OnEnemyKilled -= OnEnemyKilled;
    }

    void Start()
    {
        EnemyManager.Instance.maxEnemyCount = currentStage.maxEnemyCount;
    }

    void OnEnemyKilled()
    {
        killCount++;

        if (killCount >= currentStage.killTarget)
        {
            GameEvents.OnStageClear?.Invoke();
        }
    }
}