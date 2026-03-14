using UnityEngine;

public class BossAttackState : BossState
{
    float attackCooldown = 2f;
    float timer;

    public override void Enter(BossBrain brain)
    {
        brain.animator.SetBool("isMove", false);
        timer = 0f;
    }

    public override void Tick(BossBrain brain)
    {
        float dist = Vector3.Distance(brain.transform.position, brain.player.position);

        if (dist > brain.attack.attackRange)
        {
            brain.ChangeState(new BossChaseState());
            return;
        }

        timer += brain.tickInterval;

        if (timer >= attackCooldown)
        {
            timer = 0f;
            brain.attack.DoAttack(brain.animator);
        }
    }

    public override void Exit(BossBrain brain)
    {
    }
}