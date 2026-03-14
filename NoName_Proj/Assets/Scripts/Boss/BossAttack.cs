using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float attackRange = 3f;

    int attackStack = 0;

    public void DoAttack(Animator anim)
    {
        if (attackStack == 0)
        {
            anim.Play("Attack01");
        }
        else
        {
            anim.Play("Attack02");
        }

        attackStack++;

        if (attackStack >= 2)
            attackStack = 0;
    }
}