using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class ControllerBase : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float runSpeed = 6f; 

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;
    private float yaw = 0f;
    private float pitch = 0f;

    [SerializeField] private Transform cameraTransform; 
    protected float MoveSpeed => moveSpeed;
    protected float RotationSpeed => rotationSpeed;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    protected virtual void Update()
    {
        if (!GameSystem.START_GAME || Controller.Instance.LockControl) return;

        HandleMouseLook();                  
        ApplyGravity();          
        HandleRotation();        
    }

    protected virtual void FixedUpdate()
    {
        HandleMovement();        
    }


    protected virtual void HandleMovement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        if (input.sqrMagnitude < 0.01f)
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
            return;
        }

        Vector3 move = transform.right * input.x + transform.forward * input.z;
        controller.Move(move * currentMoveSpeed * Time.fixedDeltaTime);

        float currentSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }
    }



    protected virtual void ApplyGravity()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    protected virtual void HandleRotation()
    {
        Vector3 direction = cameraTransform.forward;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -40f, 75f);

        if (cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
    }

    public virtual void Init()
    {
        enabled = true;
    }

    public virtual void Disable()
    {
        enabled = false;
    }
}
