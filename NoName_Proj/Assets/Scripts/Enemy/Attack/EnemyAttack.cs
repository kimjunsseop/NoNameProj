using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    protected Enemy enemy;

    public virtual void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public abstract void Attack();
}