using UnityEngine;

public class BossBrain : MonoBehaviour
{
    public BossState currentState;

    public Transform player;
    public BossMovement movement;
    public BossAttack attack;
    public Animator animator;

    public float tickInterval = 0.2f;
    float timer;
    void OnEnable()
    {
        GameEvents.OnPlayerSpawned += SetTarget;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerSpawned -= SetTarget;
    }

    void SetTarget(Transform player)
    {
        this.player = player;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tickInterval)
        {
            timer = 0f;
            currentState?.Tick(this);
        }
    }

    public void ChangeState(BossState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }
    
}