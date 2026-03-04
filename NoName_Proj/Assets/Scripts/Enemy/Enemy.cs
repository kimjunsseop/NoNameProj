using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Dead
}

public class Enemy : MonoBehaviour
{
    public EnemyState state;
    void OnEnable()
    {
        
    }
    void Tick()
    {
        
    }
}
