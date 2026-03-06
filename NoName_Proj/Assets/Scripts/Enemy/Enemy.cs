using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("Stats")]
    public int maxHp = 10;
    public int attackPower = 2;
    public float chaseRange = 10f;
    public float attackRange = 2f;

    private int currentHp;

    public Transform target; // 테스트용 public
    private NavMeshAgent agent;
    private Animator anim;

    public event Action<Enemy> OnDeath;
    [Header("Test")] // 테스트용으로 EnemyManager에 등록. 나중엔 Spawner에서 활성화 시키면서 거기서 등록 시키도록 변경
    public GameObject MonsterManager;
    private EnemyManager manager;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        manager = MonsterManager.GetComponent<EnemyManager>();
        manager.RegisterEnemy(this);
    }

    void OnEnable()
    {
        currentHp = maxHp;
        ChangeState(EnemyState.Idle);
    }

    public void Initialize(Transform target)
    {
        // 테스트로 일단 바로 타겟 inspector에서 대입
        // 나중엔 spawner에서 등록할 예정
        if(target != null) return;
        this.target = target;
    }

    public void Tick()
    {
        if (state == EnemyState.Dead) return;

        switch (state)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;

            case EnemyState.Chase:
                UpdateChase();
                break;

            case EnemyState.Attack:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist < chaseRange)
        {
            ChangeState(EnemyState.Chase);

        }
    }

    void UpdateChase()
    {
        if (target == null) return;

        agent.SetDestination(target.position);

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist < attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    void UpdateAttack()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist > attackRange)
        {
            ChangeState(EnemyState.Chase);
        }

        // 공격 애니메이션에서 실제 공격 발생
    }

    void ChangeState(EnemyState newState)
    {
        if (state == newState) return;

        state = newState;

        switch (state)
        {
            case EnemyState.Idle:
                EnterIdle();
                break;

            case EnemyState.Chase:
                EnterChase();
                break;

            case EnemyState.Attack:
                EnterAttack();
                break;

            case EnemyState.Dead:
                EnterDead();
                break;
        }
    }

    void EnterIdle()
    {
        agent.isStopped = true;
        anim.SetBool("isMove", false);
        anim.SetBool("isAttack", false);
    }

    void EnterChase()
    {
        agent.isStopped = false;
        anim.SetBool("isMove", true);
        anim.SetBool("isAttack", false);                   
    }

    void EnterAttack()
    {
        agent.isStopped = true;

        anim.SetBool("isMove", false);
        anim.SetBool("isAttack", true);
    }

    void EnterDead()
    {
        agent.isStopped = true;
        anim.Play("Die");
    }

    public void TakeDamage(int damage)
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

        agent.isStopped = true;

        OnDeath?.Invoke(this);

        gameObject.SetActive(false);
    }
}
