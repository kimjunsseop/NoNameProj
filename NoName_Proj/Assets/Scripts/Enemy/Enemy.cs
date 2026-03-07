using System;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Dead
}

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyState state;

    [Header("Data")]
    public EnemyData data;

    [Header("Components")]
    public EnemyBrain brain;
    public EnemyMovement movement;
    public EnemyAttack attack;

    private float currentHp;

    public Transform target;

    public event Action<Enemy> OnDeath;

    private EnemyManager manager;

    void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
    }

    public void Initialize(Transform target, EnemyManager manager)
    {
        this.target = target;
        this.manager = manager;

        manager.RegisterEnemy(this);

        brain.Initialize(this);
        movement.Initialize(this);
        attack.Initialize(this);
    }

    void OnEnable()
    {
        currentHp = data.maxHp;
        ChangeState(EnemyState.Idle);
    }

    public void Tick()
    {
        if (state == EnemyState.Dead) return;

        brain.Tick();
    }

    public void ChangeState(EnemyState newState)
    {
        if (state == newState) return;

        state = newState;
        brain.OnStateEnter(newState);
    }

    public void TakeDamage(float damage)
    {
        if (state == EnemyState.Dead) return;

        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ChangeState(EnemyState.Dead);

        OnDeath?.Invoke(this);

        PoolManager.Instance.Return(gameObject);
    }
}