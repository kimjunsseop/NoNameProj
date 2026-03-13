using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public int level = 1;
    public int currentExp = 0;

    public int maxHp = 100;
    public int currentHp;

    public event Action<int,int> OnHpChanged;
    public event Action<int> OnExpChanged;

    void Awake()
    {
        currentHp = maxHp;
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        OnExpChanged?.Invoke(currentExp);
    }

    public void SpendExp(int amount)
    {
        currentExp -= amount;

        if (currentExp < 0)
            currentExp = 0;

        OnExpChanged?.Invoke(currentExp);
    }

    public void AddHP(int amount)
    {
        if(amount + currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        else
        {
            currentHp += amount;
        }

        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    public void ExpandHP(int amount)
    {
        maxHp += amount;
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

     public void TakeDamage(float damage)
    {
        currentHp -= (int)damage;

        if (currentHp < 0)
            currentHp = 0;

        OnHpChanged?.Invoke(currentHp, maxHp);

        if (currentHp == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");

        GameEvents.OnPlayerDead?.Invoke();
    }
}