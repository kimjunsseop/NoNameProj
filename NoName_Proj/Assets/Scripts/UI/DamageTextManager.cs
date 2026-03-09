using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance { get; private set; }

    public GameObject damageTextPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowDamage(int damage, Vector3 pos)
    {
        GameObject obj = PoolManager.Instance.Get(damageTextPrefab);

        DamageText dt = obj.GetComponent<DamageText>();

        dt.Show(damage, pos);
    }
}