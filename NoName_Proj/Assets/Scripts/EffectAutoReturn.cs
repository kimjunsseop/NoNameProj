using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EffectAutoReturn : MonoBehaviour
{
    private ParticleSystem ps;
    private Poolable poolable;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        poolable = GetComponent<Poolable>();
    }

    void OnEnable()
    {
        ps.Play();
    }

    void OnParticleSystemStopped()
    {
        if (poolable != null)
        {
            PoolManager.Instance.Return(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}