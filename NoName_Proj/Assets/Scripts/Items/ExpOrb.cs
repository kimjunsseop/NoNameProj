using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ExpOrb : MonoBehaviour
{
    [Header("Exp Settings")]
    public int expValue = 1;
    public float moveSpeed = 5f;
    public float accelerate = 10f;

    [Header("Attract Settings")]
    public float attractDistance = 3f;
    public float pickupDistance = 0.2f;

    private Transform player;
    private bool isAttracting = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;          // 튀기기 후 자연스럽게 떨어지도록
        rb.isKinematic = false;

        SphereCollider sc = GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = attractDistance;
    }

    private void OnEnable()
    {
        // 초기 속도, 튀기기
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        rb.AddForce(Vector3.up * 5f + randomDir, ForceMode.Impulse);

        moveSpeed = 5f;
        isAttracting = false;
        player = null;
    }

    /// <summary>
    /// Enemy에서 드랍 시 설정
    /// </summary>
    public void Initialize(int value)
    {
        expValue = value;
        moveSpeed = 5f;
        isAttracting = false;
        player = null;
    }

    private void Update()
    {
        if (!isAttracting || player == null) return;

        // Rigidbody.MovePosition으로 부드럽게 플레이어 추적
        Vector3 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);

        moveSpeed += accelerate * Time.deltaTime;

        // 경험치 흡수
        if (Vector3.Distance(rb.position, player.position) < pickupDistance)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if (stats != null)
                stats.AddExp(expValue);

            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isAttracting = true;

            // Rigidbody 추적용
            rb.isKinematic = false; // MovePosition과 충돌 없이 이동 가능
        }
    }

    private void ReturnToPool()
    {
        PoolManager.Instance.Return(gameObject);
    }
}