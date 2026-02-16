using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerModel : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 0.2f;
    [SerializeField] private Transform groundCheck;

    [Header("Input")]
    [SerializeField] private InputActionReference movementAction;
    [SerializeField] private InputActionReference jumpAction;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        movementAction.action.Enable();
        jumpAction.action.Enable();

        jumpAction.action.started += Jump;
    }

    void Update()
    {
        HandleInput();
        CheckGround();
        HandleCoyoteTime();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
        movement = movementAction.action.ReadValue<Vector2>();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (coyoteTimeCounter <= 0f)
            return;

        coyoteTimeCounter = 0f;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            rayDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;

        Debug.DrawRay(
            groundCheck.position,
            Vector2.down * rayDistance,
            isGrounded ? Color.green : Color.red
        );
    }

    private void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        jumpAction.action.started -= Jump;
    }
}
