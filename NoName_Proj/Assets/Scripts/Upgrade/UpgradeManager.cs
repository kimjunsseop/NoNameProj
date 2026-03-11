using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeData> upgradePool;

    GameObject player;

    void OnEnable()
    {
        GameEvents.OnUpgradeSelected += ApplyUpgrade;
    }

    void OnDisable()
    {
        GameEvents.OnUpgradeSelected -= ApplyUpgrade;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void ApplyUpgrade(UpgradeData data)
    {
        data.effect.Apply(player);
    }

    public List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> result = new();

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, upgradePool.Count);

            result.Add(upgradePool[index]);
        }

        return result;
    }
}