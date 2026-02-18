using System;
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
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    [Header("Attack Properties")] 
    [SerializeField]private float attackDamage;
    [SerializeField]private float attackRadius;
    [SerializeField] private LayerMask attackLayer;
    public Transform attackPoint;
    
    private Rigidbody2D rb;
    
    private Vector2 movementInput;

    private bool isGrounded;
    private bool wasGrounded;

    
    // EVENTS → Para Visuals
    public event Action OnJumped;
    public event Action<bool,float> OnMoveStateChanged;
    public event Action<bool> OnGroundStateChanged;

    public event Action OnKick;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        CheckGround();
        HandleCoyoteTime();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    public void SetMovement(Vector2 input)
    {
        movementInput = input;

        bool isMoving = Mathf.Abs(input.x) > 0.01f;
        OnMoveStateChanged?.Invoke(isMoving,input.x);
    }

    public void RequestKick()
    {
        OnKick?.Invoke();
        
        bool isAbleToMakeDamage= Physics2D.OverlapCircle(transform.position, attackRadius,attackLayer);
        if (isAbleToMakeDamage)
        {
            
        }
    }

    public void RequestJump()
    {
        if (coyoteTimeCounter > 0f)
        {
            coyoteTimeCounter = 0f;
            PerformJump();
        }
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(movementInput.x * speed, rb.linearVelocity.y);
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        OnJumped?.Invoke();
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded != wasGrounded)
        {
            OnGroundStateChanged?.Invoke(isGrounded);
            wasGrounded = isGrounded;
        }
    }

    private void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }
} 