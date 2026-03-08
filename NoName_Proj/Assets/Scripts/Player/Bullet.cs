using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float damage;

    Rigidbody rb;
    Poolable poolable;
    public GameObject hitEffectPrefab;
    public float lifeTime = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        poolable = GetComponent<Poolable>();
    }

    public void Init(float bulletSpeed, float bulletDamage)
    {
        speed = bulletSpeed;
        damage = bulletDamage;

        rb.linearVelocity = transform.forward * speed;

        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SpawnHitEffect();
            IDamageable d = other.GetComponent<IDamageable>();

            if (d != null)
            {
                d.TakeDamage(damage);
            }

            ReturnToPool();
        }
    }
    void SpawnHitEffect()
    {
        GameObject effect = PoolManager.Instance.Get(hitEffectPrefab);

        effect.transform.position = transform.position;
        effect.transform.rotation = Quaternion.identity;
    }
    void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero;

        PoolManager.Instance.Return(gameObject);
    }
}