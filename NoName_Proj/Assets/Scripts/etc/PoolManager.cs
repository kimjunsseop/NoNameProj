using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PoolManager");
                instance = go.AddComponent<PoolManager>();

                // DDOL로 갈지 말지 모르겠네. 일단 하지말아봐
                //DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    private Dictionary<GameObject, ObjectPool> pools
        = new Dictionary<GameObject, ObjectPool>();


    public void CreatePool(GameObject prefab, int size)
    {
        if (pools.ContainsKey(prefab)) return;

        ObjectPool pool = new ObjectPool(prefab, size, transform);
        pools.Add(prefab, pool);
    }


    public GameObject Get(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            CreatePool(prefab, 10);
        }

        return pools[prefab].Get();
    }


    public void Return(GameObject obj)
    {
        Poolable poolable = obj.GetComponent<Poolable>();

        if (poolable != null)
        {
            poolable.ReturnToPool();
        }
        else
        {
            Destroy(obj);
        }
    }
}