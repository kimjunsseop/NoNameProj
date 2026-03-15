using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float damage;

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
        damage = bulletDamage;

        rb.linearVelocity = transform.forward * speed;

        ApplyDamageVisual();

        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void ApplyDamageVisual()
    {
        if (meshRenderer == null)
            return;

        meshRenderer.GetPropertyBlock(mpb);

        // 데미지 기반 색 변화
        float t = Mathf.InverseLerp(5f, 50f, damage);
        Color color = Color.Lerp(Color.white, Color.red, t);

        mpb.SetColor("_BaseColor", color);

        meshRenderer.SetPropertyBlock(mpb);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SpawnEnemyHitEffect();
            IDamageable d = other.GetComponent<IDamageable>();

            if (d != null)
            {
                d.TakeDamage(damage);
            }

            ReturnToPool();
        }

        if (other.CompareTag("Boss"))
        {
            //SpawnBossHitEffect(); 이거 따로 만들기
            IDamageable d = other.GetComponent<IDamageable>();

            if (d != null)
            {
                d.TakeDamage(damage);
            }

            ReturnToPool();
        }
    }
    
    void SpawnEnemyHitEffect()
    {
        GameObject effect = PoolManager.Instance.Get(hitEffectPrefab);

        effect.transform.position = transform.position + new Vector3(0, -1f, 0);
        effect.transform.rotation = Quaternion.identity;
    }

    void SpawnBossHitEffect()
    {
        GameObject effect = PoolManager.Instance.Get(bossHitEffectPrefab);
        effect.transform.position = transform.position + new Vector3(0, 1f, 0);
        effect.transform.rotation = Quaternion.identity;
    }
    void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero;

        PoolManager.Instance.Return(gameObject);
    }
}