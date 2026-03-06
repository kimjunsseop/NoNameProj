using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void OnEnable()
    {
        EnemyManager.Instance.RegisterSpawner(this);
    }

    void OnDisable()
    {
        EnemyManager.Instance.UnregisterSpawner(this);
    }
    public Enemy Spawn(Enemy prefab, Transform target)
    {
        Enemy enemy = Instantiate(prefab, transform.position, Quaternion.identity);

        enemy.Initialize(target);

        return enemy;
    }
}