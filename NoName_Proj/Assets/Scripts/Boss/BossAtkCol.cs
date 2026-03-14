using UnityEngine;

public class BossAtkCol : MonoBehaviour
{
    public float damage = 20f;

    void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}