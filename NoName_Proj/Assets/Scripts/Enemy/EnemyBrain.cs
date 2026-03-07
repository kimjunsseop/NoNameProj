using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    private Enemy enemy;

    public void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Tick()
    {
        switch (enemy.state)
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

    public void OnStateEnter(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                enemy.movement.Stop();
                break;

            case EnemyState.Chase:
                break;

            case EnemyState.Attack:
                enemy.movement.Stop();
                break;
        }
    }

    void UpdateIdle()
    {
        if (enemy.target == null) return;

        float dist = Vector3.Distance(
            enemy.transform.position,
            enemy.target.position);

        if (dist < enemy.data.chaseRange)
        {
            enemy.ChangeState(EnemyState.Chase);
        }
    }

    void UpdateChase()
    {
        if (enemy.target == null) return;

        enemy.movement.MoveTo(enemy.target.position);

        float dist = Vector3.Distance(
            enemy.transform.position,
            enemy.target.position);

        if (dist < enemy.data.attackRange)
        {
            enemy.ChangeState(EnemyState.Attack);
        }
    }

    void UpdateAttack()
    {
        if (enemy.target == null) return;

        float dist = Vector3.Distance(
            enemy.transform.position,
            enemy.target.position);

        if (dist > enemy.data.attackRange)
        {
            enemy.ChangeState(EnemyState.Chase);
            return;
        }

        enemy.attack.Attack();
    }
}