using UnityEngine;

public class MeleeAttack : EnemyAttack
{
    private float lastAttackTime;
    public float attackCooldown = 1f;

    public override void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        enemy.animator.SetBool("isAttack", true);

        if (enemy.target == null) return;

        IDamageable damageable =
            enemy.target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(enemy.data.attackPower);
        }
    }
}