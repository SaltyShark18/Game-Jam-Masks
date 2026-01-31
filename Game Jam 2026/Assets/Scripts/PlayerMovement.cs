using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    float speedMultiplyer = 1f;

    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;
    public static bool dashUnlocked = false;


    [Header("Jumping")]
    public float jumpPower = 10f;
    public static int maxJumps = 2;
    int jumpsRemaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;
    bool isOnPlatform;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("Wall Movement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;

    //Wall Jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);
    public static bool wallJumpUnlocked = false;

    void Start()
    {
        // GameController.SpeedUpgraded += SpeedIncrease;
        // GameController.DashUnlocked += UnlockDash;
        // GameController.DoubleJumpUnlocked += UnlockDoubleJump;
        // GameController.WallJumpUnlocked += UnlockWallJump;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }
        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed * speedMultiplyer, rb.velocity.y);
            Flip();
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        animator.SetBool("isWallsliding", isWallSliding);
    }

    public void Move(InputAction.CallbackContext contxet)
    {
        horizontalMovement = contxet.ReadValue<Vector2>().x;
    }

    // public void SpeedIncrease(float multiplyer)
    // {
    //     speedMultiplyer = 1 + multiplyer;
    // }

    public void Dash(InputAction.CallbackContext contxet)
    {
        if (contxet.performed && canDash /*&& dashUnlocked*/)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        //If Want Invincibility
        //Physics2D.IgnoreLayerCollision(7, 8, true);
        canDash = false;
        isDashing = true;

        float dashDirection = isFacingRight ? 1f : -1f;

        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = new Vector2(0f, rb.velocity.y);

        isDashing = false;
        //If Want Invincibility
        //Physics2D.IgnoreLayerCollision(7, 8, false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // void UnlockDash()
    // {
    //     dashUnlocked = true;
    // }

    public void Jump(InputAction.CallbackContext contxet)
    {
        if (jumpsRemaining > 0)
        {
            if (contxet.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
            else if (contxet.canceled && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
        }

        //Wall Jump
        if (contxet.performed && wallJumpTimer > 0f /*&& wallJumpUnlocked*/)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;
            animator.SetTrigger("jump");
            
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }
    }

    // void UnlockDoubleJump()
    // {
    //     maxJumps = 2;
    // }

    // void UnlockWallJump()
    // {
    //     wallJumpUnlocked = true;
    // }

    public void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    public void ProcessGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ProcessWallSlide()
    {
        if (!isGrounded & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.blue;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
