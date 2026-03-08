using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public EnemyDetector detector;
    public Gun gun;

    public float attackRate = 0.5f;

    float timer;

    void Update()
    {
        if (!detector.HasEnemy())
            return;

        timer += Time.deltaTime;

        if (timer >= attackRate)
        {
            timer = 0f;
            gun.Shoot();
        }
    }
}