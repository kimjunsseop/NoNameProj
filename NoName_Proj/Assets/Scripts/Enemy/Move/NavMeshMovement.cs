using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovement : EnemyMovement
{
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Initialize(Enemy enemy)
    {
        base.Initialize(enemy);

        agent.speed = enemy.data.moveSpeed;
    }

    public override void MoveTo(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
    }

    public override void Stop()
    {
        agent.isStopped = true;
    }
}