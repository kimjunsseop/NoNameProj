using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public int Count { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Count++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Count--;
            if (Count < 0) Count = 0;
        }
    }

    public bool HasEnemy()
    {
        return Count > 0;
    }
}