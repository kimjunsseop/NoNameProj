using UnityEngine;

public class Gun : MonoBehaviour
{
    public EnemyDetector detector;

    public Transform muzzle;

    public GameObject muzzleFlashPrefab;
    public GameObject impactEffectPrefab;
    public GameObject bulletTrailPrefab;

    public float damage = 10f;
    public float range = 50f;

    public float fireInterval = 0.5f;

    float fireTimer;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (!detector.HasEnemy()) return;

        if (fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0;
        }
    }

    void Fire()
    {
        Ray ray = new Ray(muzzle.position, muzzle.forward);

        RaycastHit hit;

        Vector3 hitPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            hitPoint = hit.point;

            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            SpawnImpact(hit.point, hit.normal);
        }
        else
        {
            hitPoint = ray.origin + ray.direction * range;
        }
        Debug.DrawRay(muzzle.position, muzzle.forward * range, Color.red, 2f);
        SpawnMuzzleFlash();
        SpawnBulletTrail(hitPoint);
    }

    void SpawnMuzzleFlash()
    {
        if (muzzleFlashPrefab != null)
        {
            Instantiate(muzzleFlashPrefab, muzzle.position, muzzle.rotation);
        }
    }

    void SpawnImpact(Vector3 point, Vector3 normal)
    {
        if (impactEffectPrefab != null)
        {
            Quaternion rot = Quaternion.LookRotation(normal);
            Instantiate(impactEffectPrefab, point, rot);
        }
    }

    void SpawnBulletTrail(Vector3 hitPoint)
    {
        if (bulletTrailPrefab != null)
        {
            GameObject trail = Instantiate(bulletTrailPrefab, muzzle.position, muzzle.rotation);

            BulletTrail trailScript = trail.GetComponent<BulletTrail>();

            if (trailScript != null)
            {
                trailScript.Init(hitPoint);
            }
        }
    }
}