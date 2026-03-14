using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Animator anim;
    public EnemyDetector detector;
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] private float jumpPower = 3f;
    private Vector2 move;
    private bool isJumpButton;
    private bool currentGround;
    private bool wasGround;
    [SerializeField] private float GroundCheckDis = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Camera cam;
    [SerializeField] private float rotationSpeed = 180;
    private Vector3 targetLookPos;
    void Awake()
    {
        playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (cam == null)
            cam = Camera.main;
    }
    void OnEnable()
    {
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OnMove;
        playerInput.Player.Jump.performed += OnJump;
        playerInput.Player.Jump.canceled += OnJump;
        detector.OnEnemyEnter += OnEnemyEntered;
        GameEvents.OnCameraReady += SetCamera;
        playerInput.Enable();
    }
    void OnDisable()
    {
        playerInput.Player.Move.performed -= OnMove;
        playerInput.Player.Move.canceled -= OnMove;
        playerInput.Player.Jump.performed -= OnJump;
        playerInput.Player.Jump.canceled -= OnJump;
        detector.OnEnemyEnter -= OnEnemyEntered;
        GameEvents.OnCameraReady -= SetCamera;
        playerInput.Disable();
    }
    void SetCamera(Camera c)
    {
        cam = c;
    }
    void Update()
    {
        // 마우스의 현재 위치를 받아와, Screen에다가 Ray를 쏜다
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            targetLookPos = hit.point;
        }
    }

    public void AddSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log($"real Speed : {moveSpeed}");
        GameEvents.OnMoveSpeedChanged?.Invoke(moveSpeed);
    }
    void FixedUpdate()
    {

        // 플레이어가 바라보는 방향 기준으로 앞뒤좌우 움직임
        // Vector3 moveDir = transform.forward * move.y + transform.right * move.x;
        // Vector3 velocity = rb.linearVelocity;
        // velocity.x = moveDir.x * moveSpeed;
        // velocity.z = moveDir.z * moveSpeed;
        // rb.linearVelocity = velocity;

        // World 좌표계 기준으로 플레이어 앞뒤좌우 움직임 
        Vector3 velocity = rb.linearVelocity;
        velocity.x = move.x * moveSpeed;
        velocity.z = move.y * moveSpeed;
        rb.linearVelocity = velocity;
        currentGround = Physics.Raycast(transform.position, Vector3.down, GroundCheckDis, groundLayer);
        Vector3 direction = targetLookPos - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            Quaternion newRot = Quaternion.RotateTowards(
                rb.rotation,
                targetRot,
                rotationSpeed * Time.fixedDeltaTime
            );
            rb.MoveRotation(newRot);
        }   
        if(!wasGround && currentGround)
        {
            anim.SetBool("isJump", false);
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        if(move != Vector2.zero)
        {
            anim.SetBool("isMove", true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }
    void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed && currentGround)
        {
            if(move != Vector2.zero)
            {
                move = Vector2.zero;
                anim.SetBool("isJump", true);
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
            else
            {
                anim.SetBool("isJump", true);
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
    }
    void OnEnemyEntered(bool hasEnemy)
    {
        anim.SetBool("isAttack", hasEnemy);
    }
}
