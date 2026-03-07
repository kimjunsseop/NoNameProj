using UnityEngine;

public class Poolable : MonoBehaviour
{
    public ObjectPool pool;

    public void ReturnToPool()
    {
        if (pool != null)
        {
            pool.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}