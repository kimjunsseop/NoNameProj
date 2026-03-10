using System;
using Unity.VisualScripting;
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
    private HitFlashController hitFlash;
    [Header("Orb")]
    public GameObject expOrb;
    void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
        animator = GetComponent<Animator>();
        ui = GetComponent<EnemyUI>();
        hitFlash = GetComponent<HitFlashController>();
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
        // hitFlash 발동
        hitFlash.HitFlash();

        ui.Show();
        ui.UpdateHealth(currentHp, data.maxHp);

        DamageTextManager.Instance.ShowDamage((int)damage, transform.position + Vector3.up * 2f);

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
        SpawnExpOrb();
    }
    void SpawnExpOrb()
    {
        int expAmount = data.expDrop;

        GameObject orbGO = PoolManager.Instance.Get(expOrb); // 풀에서 바로 가져오기
        orbGO.transform.position = transform.position;
        //orbGO.SetActive(true);
        
        var orb = orbGO.GetComponent<ExpOrb>();
        orb.Initialize(expAmount);

        // ExpOrb.cs에서 OnEnable될 때 Rigidbody를 사용해 튀어오르게 구현
    }

    void ReturnToPool()
    {
        PoolManager.Instance.Return(gameObject);
    }
}