using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EffectAutoReturn : MonoBehaviour
{
    private ParticleSystem ps;
    private Poolable poolable;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        poolable = GetComponent<Poolable>();
        ps.Play();
    }

    void OnParticleSystemStopped()
    {
        if (poolable != null)
        {
            Debug.Log("a");
            PoolManager.Instance.Return(gameObject);
        }
        else
        {
            Debug.Log("b");
            gameObject.SetActive(false);
        }
    }
}