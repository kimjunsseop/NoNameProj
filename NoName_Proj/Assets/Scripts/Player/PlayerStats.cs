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
    private Animator anim;

    void Awake()
    {
        currentHp = maxHp;
        anim = GetComponent<Animator>();
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

    public void TakeDamage(DamageInfo info)
    {
        currentHp -= (int)info.damage;

        if (currentHp < 0)
            currentHp = 0;

        OnHpChanged?.Invoke(currentHp, maxHp);

        if (info.cameraShake > 0f)
        {
            CameraShakeManager.Instance?.Shake(info.cameraShake);
        }

        if (currentHp == 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.Play("Die");
    }

    public void EndingDieAnim()
    {
        GameEvents.OnPlayerDead?.Invoke();
    }
}