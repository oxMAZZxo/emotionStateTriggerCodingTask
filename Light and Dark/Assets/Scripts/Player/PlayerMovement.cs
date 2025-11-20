using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private float moveSpeed;
    [SerializeField, Range(0.001f, 1f)] private float movementSmoothing;
    [SerializeField]private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private Rigidbody2D rb;
    private Vector3 refVelocity;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float direction = moveInput.action.ReadValue<float>();
        Vector3 targetVelocity = new Vector2(direction * moveSpeed * 10f * Time.fixedDeltaTime, rb.linearVelocity.y);
        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref refVelocity, movementSmoothing);
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position,0.1f,groundLayer);

        isGrounded = colliders.Length > 0;        
    }
    
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if(isGrounded)
        {
            rb.AddForce(new Vector2(0,jumpForce));
        }
    }

    void OnEnable()
    {
        moveInput.action.Enable();
        jumpInput.action.Enable();
        jumpInput.action.performed += OnJumpInput;
    }

    void OnDisable()
    {
        moveInput.action.Disable();
        jumpInput.action.Disable();
        jumpInput.action.performed -= OnJumpInput;
    }

    void OnDrawGizmosSelected()
    {
        if(groundCheck == null ) {return;}
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position,0.1f);
    }
}
