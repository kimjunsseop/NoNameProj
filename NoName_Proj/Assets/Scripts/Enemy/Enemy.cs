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
    public Animator animator;
    public EnemyUI ui;

    private float currentHp;

    public Transform target;

    public event Action<Enemy> OnDeath; // 매니저가 구독할거. list관리할때 지우려고

    private EnemyManager manager;

    void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
        animator = GetComponent<Animator>();
        ui = GetComponent<EnemyUI>();
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

        // var rb = GetComponent<Rigidbody>();
        // if (rb != null)
        // {
        //     rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // }

        // var col = GetComponent<Collider>();
        // if (col != null)
        // col.enabled = true;

        // var agent = GetComponent<NavMeshMovement>();
        // if (agent != null)
        //     agent.enabled = true;

        

        ChangeState(EnemyState.Idle);
    }

    void OnDisable()
    {
        OnDeath = null;
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

        ui.Show();
        ui.UpdateHealth(currentHp, data.maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ChangeState(EnemyState.Dead);

        movement.Stop();
        
        // var rb = GetComponent<Rigidbody>();
        // if (rb != null)
        // {
        //     rb.constraints = RigidbodyConstraints.None;
        // }
       
        // var col = GetComponent<Collider>();
        // if (col != null)
        //     col.enabled = false;

        // var agent = GetComponent<NavMeshMovement>();
        // if (agent != null)
        //     agent.enabled = false;

        animator.Play("Die");

        OnDeath?.Invoke(this);

        // Animation Event에서 ReturnToPool 실행.
    }

    void ReturnToPool()
    {
        PoolManager.Instance.Return(gameObject);
    }
}