using UnityEngine;

public class MeleeAttack : EnemyAttack
{
    protected override void StartAttack()
    {
        enemy.animator.SetBool("isAttack", true);
    }

    // 애니메이션 이벤트에서 호출
    public void PerformAttack()
    {
        if (enemy.target == null) return;

        IDamageable damageable =
            enemy.target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(enemy.data.attackPower);
        }
    }
}