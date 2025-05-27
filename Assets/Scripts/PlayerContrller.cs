using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    public float MouseSensitivity = 100.0f;
    public float MoveSpeed = 5.0f;
    public Transform CameraPosition;

    private CharacterController characterController;
    private float verticalAngle, horizontalAngle;
    private Vector3 playerVelocity;
    private float gravity = -9.81f;

    // Dash settings
    public float DashSpeed = 20f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;

    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        horizontalAngle = transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (!GameSystem.START_GAME || Controller.Instance.LockControl)
            return;

        HandleMouseLook();
        HandleDash();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float turnPlayer = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        horizontalAngle = (horizontalAngle + turnPlayer) % 360;

        Vector3 currentAngles = transform.localEulerAngles;
        currentAngles.y = horizontalAngle;
        transform.localEulerAngles = currentAngles;

        float turnCam = -Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle + turnCam, -89.0f, 89.0f);

        currentAngles = CameraPosition.localEulerAngles;
        currentAngles.x = verticalAngle;
        CameraPosition.localEulerAngles = currentAngles;
    }

    void HandleMovement()
    {
        if (isDashing) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(moveDirection * MoveSpeed * Time.deltaTime);

        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void HandleDash()
    {
        if (isDashing)
        {
            characterController.Move(dashDirection * DashSpeed * Time.deltaTime);
            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false;
                dashCooldownTimer = DashCooldown;
            }
            return;
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            Vector3 inputDirection = transform.right * moveX + transform.forward * moveZ;

            if (inputDirection != Vector3.zero)
            {
                dashDirection = inputDirection.normalized;
                isDashing = true;
                dashTime = DashDuration;
            }
        }
    }
}
