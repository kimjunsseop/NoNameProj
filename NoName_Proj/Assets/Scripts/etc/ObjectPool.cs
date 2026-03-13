using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private Transform parent;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    void CreateNewObject()
    {
        //Debug.Log("Pool Expand : " + prefab.name);
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.SetActive(false);

        Poolable poolable = obj.GetComponent<Poolable>();

        if (poolable == null)
        {
            poolable = obj.AddComponent<Poolable>();
        }

        poolable.pool = this;

        pool.Enqueue(obj);
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            CreateNewObject();
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}