using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float damage;

    bool isCritical;
    float critMultiplier = 2f;
    float critChance = 0.2f;

    Rigidbody rb;
    Poolable poolable;

    public GameObject hitEffectPrefab;
    public GameObject bossHitEffectPrefab;
    public float lifeTime = 3f;

    [Header("Visual")]
    [SerializeField] Renderer meshRenderer;

    MaterialPropertyBlock mpb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        poolable = GetComponent<Poolable>();

        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<Renderer>();

        mpb = new MaterialPropertyBlock();
    }

    public void Init(float bulletSpeed, float bulletDamage)
    {
        speed = bulletSpeed;

        // ✅ 크리티컬 판정
        isCritical = Random.value < critChance;
        damage = isCritical ? bulletDamage * critMultiplier : bulletDamage;

        rb.linearVelocity = transform.forward * speed;

        ApplyDamageVisual();

        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            SpawnEnemyHitEffect();

            IDamageable d = other.GetComponent<IDamageable>();

            if (d != null)
            {
                DamageInfo info = new DamageInfo
                {
                    damage = damage,
                    isCritical = isCritical,
                    hitPoint = transform.position,
                    hitDirection = transform.forward
                };

                d.TakeDamage(info);
            }

            ReturnToPool();
        }
    }

    void ApplyDamageVisual()
    {
        if (meshRenderer == null) return;

        meshRenderer.GetPropertyBlock(mpb);

        float t = Mathf.InverseLerp(5f, 50f, damage);
        Color color = isCritical
            ? Color.yellow
            : Color.Lerp(Color.white, Color.red, t);

        mpb.SetColor("_BaseColor", color);
        meshRenderer.SetPropertyBlock(mpb);
    }

    void SpawnEnemyHitEffect()
    {
        GameObject effect = PoolManager.Instance.Get(hitEffectPrefab);
        effect.transform.position = transform.position + new Vector3(0, -1f, 0);
        effect.transform.rotation = Quaternion.identity;
    }

    void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero;
        PoolManager.Instance.Return(gameObject);
    }
}