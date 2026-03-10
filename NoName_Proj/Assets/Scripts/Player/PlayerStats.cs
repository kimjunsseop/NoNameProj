using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;
    public int[] expTable; // 레벨별 필요 경험치

    public void AddExp(int amount)
    {
        currentExp += amount;
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        while(level < expTable.Length && currentExp >= expTable[level - 1])
        {
            currentExp -= expTable[level - 1];
            level++;
            Debug.Log("Level Up! " + level);
        }
    }
}