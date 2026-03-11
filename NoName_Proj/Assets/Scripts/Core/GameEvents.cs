using System;

public static class GameEvents
{
    public static Action OnEnemyKilled;

    public static Action OnStageClear;

    public static Action OnOpenUpgradeUI;

    public static Action<UpgradeData> OnUpgradeSelected;

    public static Action OnNextStage;
}