using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    // Speed
    private float horizontal;
    private float speed = 8f;

    // Jumping
    private float jumpingPower = 18f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool isJumping;

    // Turning
    private bool isFacingRight = true;

    // Walls
    public bool isWallSliding;
    private readonly float wallSlidingSpeed = 4f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    // Wall Jumping
    private bool isWallJumping;
    private float wallJumpingDir;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.16f;
    private Vector2 wallJumpingPower = new Vector2(4f, 24f);

    // Hover
    public float hoverForce = 10f;
    private bool isHovering = false;

    //reset gravity
    private float originalGravityScale;

    //coyote time
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    //jump buffer
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    // Double jump variables
    private int remainingJumps;
    public int maxJumps = 2; // Adjust this value as needed
    private bool canDoubleJump = true;

    //ground attack
    private bool isAttacking = false;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public float attackPower;

    // Air Attack
    private Vector2 airAttackDir;
    public float airAttackPower = 16f; // Adjust this value as needed
    //private bool isAirAttacking;
   // private bool canAirAttack = true;

    // Dash
    private bool isDashing;
    public float dashPower = 20f;
    public float dashDuration = 0.5f; // Adjust this value as needed
    private bool canDash = true;
    private Vector2 dashDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale; // Cache the original gravity scale.
        remainingJumps = maxJumps;
    }

    private void Update()
    {
        

        if (IsGrounded())
        {
            // Reset remaining jumps when on the ground
            remainingJumps = maxJumps;
            //set coyote time
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            //decrement time for coyote jump
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Check player's direction and adjust sprite accordingly
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        WallSlide(); // Check for wall sliding.

        Hover(); // Handle hovering.

        // Update the jump buffer counter
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Check for dash input
        if (isDashing && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        // Move the player horizontally (not during wall jump or hovering)
        if (!isWallJumping && !isHovering)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        // Jump buffer logic
        if (ctx.performed)
        {
            jumpBufferCounter = jumpBufferTime;
        }

        // Update the jump buffer counter
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Check for the jump input state
        if (ctx.started && (coyoteTimeCounter > 0f || remainingJumps > 0 || jumpBufferCounter > 0))
        {
            // Ground jump
            if (IsGrounded())
            {
                JumpStart(jumpingPower * 0.8f); // Adjust this value for shorter ground jumps
            }
            else if (remainingJumps > 0)
            {
                // Air jump
                JumpStart(jumpingPower * 0.8f); // Adjust this value for shorter air jumps
            }
        }

        // Check for the jump input release
        if (ctx.canceled && rb.velocity.y > 0)
        {
            // Reduce the upward velocity for shorter jumps when the button is released
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // Wall jump
        if (ctx.performed && isWallSliding)
        {
            GetComponent<HealthAndStamina>().TakeStamina(20);
            WallJump();
        }

        // Double jump
        if (ctx.started && (coyoteTimeCounter > 0f || (remainingJumps > 0 && canDoubleJump) || jumpBufferCounter > 0))
        {
           
            if (IsGrounded())
            {
                JumpStart(jumpingPower * 0.8f);
            }
            else if (remainingJumps > 0 && canDoubleJump)
            {
                GetComponent<HealthAndStamina>().TakeStamina(30);
                JumpStart(jumpingPower * 0.8f);
                canDoubleJump = false; // Disable double jump until grounded again
            }

            if(GetComponent<HealthAndStamina>().stamAmount <= 29)
            {
                canDoubleJump = false;
            }
        }

        // If player stops hovering
        if (ctx.canceled)
        {
            StopHover();
            RestoreOriginalGravityScale(); // Restore the original gravity scale.
        }

        
    }

    private void JumpStart(float jumpVelocity)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

        isHovering = false; // Reset hover when jumping
        isJumping = true;

        // Reset coyote time to a small positive value
        coyoteTimeCounter = 0.1f;

        // Deduct jump count for ground jump or air jump
        if (IsGrounded())
        {
            remainingJumps = maxJumps - 1;
        }
        else
        {
            remainingJumps--;
        }

        // Start hovering
        if (!IsGrounded() && !isWallSliding && remainingJumps <= 0)
        {
            StartHover();
            rb.gravityScale = 0.5f;
        }

    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        // Check for wall sliding conditions
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            //take stamina
            GetComponent<HealthAndStamina>().stamAmount -= 1f * Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 1f);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }

        if (GetComponent<HealthAndStamina>().stamAmount <= 0)
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        // Handle wall jump logic
        if (isWallSliding)
        {
            isWallJumping = false;
            // Indicates which way our character is facing
            wallJumpingDir = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            // Gives time to still wall jump when turned away
            wallJumpingCounter -= Time.deltaTime;
        }

        if (wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDir)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        // Flip the player's sprite when changing direction
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        // Read input for horizontal movement
        horizontal = ctx.ReadValue<Vector2>().x;
        Debug.Log("Horizontal: " + horizontal);
    }

    private void Hover()
    {
        // Apply hover force if hovering
        if (isHovering)
        {
            GetComponent<HealthAndStamina>().stamAmount -= 10f * Time.deltaTime;
            // Preserve horizontal movement while hovering
            rb.velocity = new Vector2(horizontal * speed, -hoverForce);
        }
        else
        {
            StopHover();
        }

        if(GetComponent<HealthAndStamina>().stamAmount <= 0)
        {
            isHovering = false;
        }
    }

    private void StartHover()
    {
        // Start hovering
        isHovering = true;
    }

    private void StopHover()
    {
        // Stop hovering
        isHovering = false;
    }

    private void RestoreOriginalGravityScale()
    {
        // Restore the original gravity scale
        rb.gravityScale = originalGravityScale;
    }

    public void GroundAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && IsGrounded())
        {
            isAttacking = true;

            if(isAttacking == true)
            {
                Debug.Log("attacking");
                //check for enemy
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

                //deal damage
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("hit");
                }
                GroundAttackCooldown();
            }
            else
            {
                //do nothing
            }
        }
    }

    public void AirAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isDashing = true;
            dashDirection = new Vector2(horizontal, 0f);
            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0f);
            }
        }
        else if (ctx.canceled)
        {
            isDashing = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        float dashTimer = 0f;

        while (dashTimer < dashDuration)
        {
            rb.velocity = new Vector2(dashDirection.normalized.x * dashPower, rb.velocity.y);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;

        // Add a cooldown before the player can dash again
        yield return new WaitForSeconds(1f);
        canDash = true;
    }

    IEnumerator GroundAttackCooldown()
    {
        // Wait for half a second
        yield return new WaitForSeconds(0.5f);

        // Reset the attacking state
        isAttacking = false;
    }
}