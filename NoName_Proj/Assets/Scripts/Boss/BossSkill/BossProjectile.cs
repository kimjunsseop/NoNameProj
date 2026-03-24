using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float height = 5f;
    public float duration = 1.2f;

    public float explodeRadius = 2f;
    public float damage = 30f;

    public GameObject impactParticle;

    Vector3 start;
    Vector3 target;

    float time;
    bool isMoving = false;
    Transform targetTransform;
    float trackingTime;
    bool isLocked = false;

    public void Init(Transform target, float trackingTime)
    {
        start = transform.position;
        this.targetTransform = target;
        this.trackingTime = trackingTime;
        time = 0f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        time += Time.deltaTime;

        // trackingTime 동안은 계속 타겟 갱신
        if (!isLocked)
        {
            if (time < trackingTime)
            {
                target = targetTransform.position;
            }
            else
            {
                // 타겟 고정
                target = targetTransform.position;
                isLocked = true;

                // 시작점 다시 설정 (부드럽게 이어지게)
                start = transform.position;
                time = 0f;
            }
        }

        float t = time / duration;

        // 포물선
        Vector3 mid = (start + target) / 2 + Vector3.up * height;

        Vector3 p1 = Vector3.Lerp(start, mid, t);
        Vector3 p2 = Vector3.Lerp(mid, target, t);

        transform.position = Vector3.Lerp(p1, p2, t);

        if (t >= 1f)
        {
            Explode();
        }
    }

    void Explode()
    {
        // 파티클
        if (impactParticle != null)
            Instantiate(impactParticle, transform.position, Quaternion.identity);

        // 데미지
        Collider[] hits = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (var hit in hits)
        {
            IDamageable d = hit.GetComponent<IDamageable>();

            if (d != null)
            {
                d.TakeDamage(new DamageInfo
                {
                    damage = damage,
                    hitPoint = transform.position,
                    hitDirection = (hit.transform.position - transform.position).normalized
                });
            }
        }

        Destroy(gameObject);
    }
}