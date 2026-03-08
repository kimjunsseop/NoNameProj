using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public GameObject bulletPrefab;
    public GameObject muzzleFlashPrefab;

    public float bulletSpeed = 20f;
    public float bulletDamage = 10f;

    public void Shoot()
    {
        SpawnMuzzleFlash();
        GameObject bulletObj = PoolManager.Instance.Get(bulletPrefab);

        bulletObj.transform.position = muzzle.position;
        bulletObj.transform.rotation = muzzle.rotation;

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        bullet.Init(bulletSpeed, bulletDamage);
    }

    void SpawnMuzzleFlash()
    {
        GameObject effect = PoolManager.Instance.Get(muzzleFlashPrefab);

        effect.transform.position = muzzle.position;
        effect.transform.rotation = muzzle.rotation;
    }
}