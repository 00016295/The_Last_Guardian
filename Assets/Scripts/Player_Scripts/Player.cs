using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
public class Player : MonoBehaviour
{
    // Health variables
    public Slider healthBar;


    // Components
    public int maxHealth = 100;
    private Rigidbody2D rb;
    private bool isGround;
    private Animator animator;
    private bool isBlocking;


    //movement variables
    public float speed = 7f;
    public float movement;
    public float jumpForce = 10f;

    //dodge variables
    public bool isDodging;
    private float dodgeDirection;
    public float dodge_force = 15f;
    public float dodge_duration = 0.5f;

    // Combo attack variables
    private int comboStep = 0;
    private float lastClickTime;
    public float comboDelay = 0.5f;

    // Attack variables
    public Transform attackPoint;
    public Transform heavyAttackPoint;
    public float attackradius = 1f;
    public float heavyAttackRadius = 0.5f;
    public LayerMask whatIsEnemy;



    // Ground check variables
    public Transform groundCheckPoint; 
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer; 

    // Double Jump variables
    public int jump_Count = 2; 
    private int total_Jumps; 

    // Direction variables
    private bool isFacingRight;

    //Wall Climb variables
    public Transform wallCheckPoint;  
    public float wallCheckRadius = 0.2f;
    public LayerMask wallLayer;     
    public float climbSpeed = 5f;

    //Wall Check variables
    private bool isTouchingWall;     
    private bool isClimbing;
    private float wallJumpTimer;

    //Ledge Grab variables
    public Transform ledgeCheckPoint;
    private bool isLedgeDetected;
    private bool isLedgeClimbing;
    public Vector2 ledgeOffset = new Vector2(0.5f, 2f);


    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        movement = 0f;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        isFacingRight = true;
        isGround = true;
        total_Jumps = jump_Count;
        animator = this.gameObject.GetComponent<Animator>();
        isBlocking = false;
        isDodging = false; 
    }

   

    void Update()
    {
        if (maxHealth <= 0)
        {
            Die(); 
        }

        if (isLedgeClimbing) return;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_pullup"))
        {
            return;
        }

        isTouchingWall = Physics2D.OverlapCircle(wallCheckPoint.position, wallCheckRadius, wallLayer);
        isLedgeDetected = Physics2D.OverlapCircle(ledgeCheckPoint.position, wallCheckRadius, wallLayer);

        if (isTouchingWall && !isGround && wallJumpTimer <= 0)
        {
            if (!isLedgeDetected)
            {
                StartCoroutine(LedgeClimbRoutine());
                return;
            }
            else
            {
                isClimbing = true;
                animator.SetBool("Jump", false);
            }
        }
        else
        {
            isClimbing = false;
        }

        animator.SetBool("is_climbing", isClimbing);
        animator.SetFloat("verticalSpeed", Mathf.Abs(Input.GetAxisRaw("Vertical")));


        // Wall Jump Cooldown
        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        //Movement Scripts
        if (isBlocking)
        {
            movement = 0f;
        }
        else
        {
            movement = Input.GetAxis("Horizontal");
        }

        // Flip the player's sprite based on movement direction
        flip();

        // Get the absolute value of movement to determine if the player is moving or idle
        if (Mathf.Abs(movement) > 0f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (Mathf.Abs(movement) < 0.1f)
        {
            animator.SetFloat("Run", 0f);
        }
        // Check for jump types
        if (Input.GetKeyDown(KeyCode.Space) && !isBlocking)
        {
            if (isClimbing)
            {
                WallJump();
            }
            else
            {
                Jump();
            }
        }

        // Check if the player is touching the ground using an overlap circle
        Collider2D collInfo = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        if (collInfo != null && Mathf.Abs(rb.linearVelocityY) < 0.1f)
        {
            isGround = true;
            total_Jumps = jump_Count;
            animator.SetBool("Jump", false);
        }
        else
        {
            isGround = false;
        }

        //Attack Scripts

        if (Input.GetMouseButtonDown(1) && !isBlocking)
        {
            if (isGround && Mathf.Abs(movement) < 0.1f)
            {
                PlayHeavyAttackAnimations();
            }
            else
            {
                Debug.Log("Тяжелая атака в движении запрещена");
            }
        }

        // Check for attack input (Left Mouse Button) and play attack animations based on the combo step
        if (Input.GetMouseButtonDown(0) && !isBlocking)
        {
            Collider2D collinfo = Physics2D.OverlapCircle(attackPoint.position, attackradius, whatIsEnemy);

            if (collinfo)
            {
                if (collinfo.gameObject.GetComponent<Enemy>() != null)
                {
                    collinfo.gameObject.GetComponent<Enemy>().TakeDamage(5); // Example damage value

                }
               
            }
            if (isGround == false)
            {
                animator.SetTrigger("Jump+attack");
            }
            else if (Mathf.Abs(movement) > 0.1f)
            {
                animator.SetTrigger("run_attack");
            }
            
            else
            {
                PlayAttackAnimations();
            }
        }


        if (Time.time - lastClickTime > comboDelay)
        {
            ResetCombo();
        }

        //Input to block and dodge
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool pressedRight = Input.GetKeyDown(KeyCode.D);
        bool pressedLeft = Input.GetKeyDown(KeyCode.A); 

        // Check if the player is pressing the block key (Q) and is moving horizontally, and not currently dodging
        if (Input.GetKey(KeyCode.Q) && isGround)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isBlocking = false;
                animator.SetBool("is_blocking", false);
                animator.SetTrigger("shield");
                Collider2D collinfo = Physics2D.OverlapCircle(attackPoint.position, attackradius, whatIsEnemy);

                if (collinfo)
                {
                    if (collinfo.gameObject.GetComponent<Enemy>() != null)
                    {
                        collinfo.gameObject.GetComponent<Enemy>().TakeDamage(5); // Example damage value

                    }
                }
                
                
            }
            else if (pressedRight || pressedLeft && !isDodging)
            {
                isBlocking = false;
                animator.SetBool("is_blocking", false);


                float dir = pressedRight ? 1f : -1f;

                StartCoroutine(Dodge(dir));
                animator.SetTrigger("Roll");
            }

            // Check if the player is pressing the block key (Q) and not currently dodging
            else if (!isDodging)
            {
                isBlocking = true;
                animator.SetBool("is_blocking", true);
                movement = 0f;
            }
        }
        
        
        // Check if the player has released the block key (Q) and is currently blocking
        if (Input.GetKeyUp(KeyCode.Q) && isGround)
        {
            isBlocking = false;
            animator.SetBool("is_blocking", false);
        }
    }

    // Move the player horizontally based on input and speed
    private void FixedUpdate()
    {
        if (isLedgeClimbing)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            // Важно: на время анимации делаем тело Kinematic, чтобы оно игнорировало все силы
            if (rb.bodyType != RigidbodyType2D.Kinematic) rb.bodyType = RigidbodyType2D.Kinematic;
            return;
        }

        // Если не подтягиваемся, возвращаем Dynamic
        if (rb.bodyType == RigidbodyType2D.Kinematic && !isLedgeClimbing)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            float vertical = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(0f, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = 1f;

            if (isDodging) return; 

            
            if (!isBlocking)
            {
                rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }

    }


    // Method to flip the player's sprite based on movement direction
    void flip()
    {
        if (movement < 0f && isFacingRight == true)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFacingRight = false;
        }
        else if (movement > 0f && isFacingRight == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
    }

    //Jump Script
    void Jump()
    {
        if (total_Jumps > 0) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.linearVelocityY = jumpForce;
            total_Jumps--;
            animator.SetBool("Jump", true);
        }

        Debug.Log("Прыжок сработал! Сила: " + jumpForce);
    }

    // Wall Jump Script
    void WallJump()
    {
        wallJumpTimer = 0.3f;
        // Determine the direction of the wall jump based on the player's facing direction
        float wallJumpDirection = isFacingRight ? -1f : 1f;

        isClimbing = false;
        animator.SetBool("is_climbing", false);

        // Apply the wall jump force to the player's Rigidbody2D
        rb.linearVelocity = new Vector2(wallJumpDirection * speed, jumpForce);

        // Flip the player in the opposite direction of the wall jump
        flip();
        animator.SetBool("Jump", true);


        Debug.Log("Прыжок от стены!");
    }

    // Methods to play attack animations based on the combo step
    void PlayAttackAnimations()
    {
        lastClickTime = Time.time;
        comboStep++;

        if (comboStep == 1)
        {
            animator.SetTrigger("attack_1");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("attack_2");
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("attack_3");
            comboStep = 0;
        }


        if (comboStep > 3) comboStep = 0;

    }

    void PlayHeavyAttackAnimations()
    {
        lastClickTime = Time.time;
        comboStep++;

        if (comboStep == 1)
        {
            animator.SetTrigger("heavy_attack1");
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("heavy_attack2");
        }

        if (comboStep > 2) comboStep = 0;
    }

    public void Attack()
    {
        Collider2D collinfo = Physics2D.OverlapCircle(attackPoint.position, attackradius, whatIsEnemy);

        if (collinfo) 
        {
            if(collinfo.gameObject.GetComponent<Enemy>()!= null)
            {
                collinfo.gameObject.GetComponent<Enemy>().TakeDamage(5); // Example damage value

            }
            if (collinfo.gameObject.GetComponent<Boss>() != null)
            {
                collinfo.gameObject.GetComponent<Boss>().TakeDamage(5); // Example damage value

            }
        }
    }

    public void HeavyAttack()
    {
        Collider2D collinfo = Physics2D.OverlapCircle(heavyAttackPoint.position, heavyAttackRadius, whatIsEnemy);

        if (collinfo)
        {
            if (collinfo.gameObject.GetComponent<Enemy>() != null)
            {
                collinfo.gameObject.GetComponent<Enemy>().TakeDamage(10); // Example damage value for heavy attack

            }
            if (collinfo.gameObject.GetComponent<Boss>() != null)
            {
                collinfo.gameObject.GetComponent<Boss>().TakeDamage(10); // Example damage value for heavy attack

            }
        }
    }
    public void TakeDamage(int damage)
    {
        if(isBlocking || isDodging)
        {
            
            return; // No damage taken if blocking
        }
        if(maxHealth <= 0)
        {
            healthBar.value = 0;
        }

        if (maxHealth <= 0 || maxHealth <= 1)
        {
            Die(); 
            return;
        }
        maxHealth -= damage; // Reduce the enemy's health by the damage amount
        healthBar.value = maxHealth; // Update the health bar UI
        animator.SetTrigger("Damage"); // Trigger the "Hurt" animation in the Animator
        if (maxHealth <= 0)
        {
            return; // Call the Die method if health is zero or less
        }
    }
    void Die()
    {
        Debug.Log("Enemy has died"); // Log message for debugging purposes
        Destroy(gameObject); // Destroy the enemy game object
    }

    // Method to reset the combo step after a delay
    void ResetCombo()
    {
        comboStep = 0;

    }

    IEnumerator Dodge(float inputDir)
    {
        isDodging = true;
        dodgeDirection = inputDir > 0f ? 1f : -1f;

        if (dodgeDirection > 0 && !isFacingRight) flip();
        else if (dodgeDirection < 0 && isFacingRight) flip();

        // for the duration of the dodge, ignore collisions between the player and enemies
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), true);
        float originalGravity = rb.gravityScale;
        
        rb.linearVelocity = new Vector2(dodgeDirection * dodge_force, 0f);

        yield return new WaitForSeconds(dodge_duration);
        // After the dodge duration, re-enable collisions and reset the player's state
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), false);
        rb.gravityScale = originalGravity;
        isDodging = false;
    }
    IEnumerator LedgeClimbRoutine()
    {
        isLedgeClimbing = true;
        isClimbing = false;
        animator.SetBool("is_climbing", false);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator.Play("Player_pullup"); 

        
        yield return new WaitForSeconds(0.61f);

        float direction = isFacingRight ? 1f : -1f;
        transform.position += new Vector3(ledgeOffset.x * direction, ledgeOffset.y, 0);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero; 
        isLedgeClimbing = false;

        Debug.Log("Залез!");
    }



    // Method to handle collision with the ground and reset jumps
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            total_Jumps = jump_Count; // Reset the total jumps when the player collides with the ground
            animator.SetBool("Jump", false); // Set the "Jump" parameter in the Animator to false to end the jump animation

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            // Draw a red wire sphere at the ground check point to visualize the ground check area
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
        if (wallCheckPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheckPoint.position, wallCheckRadius);
        }

        if (ledgeCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(ledgeCheckPoint.position, wallCheckRadius);
        }

        if (attackPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(attackPoint.position, attackradius);
        }

        if (heavyAttackPoint != null)
        {
            Gizmos.color = Color.purple;
            Gizmos.DrawWireSphere(heavyAttackPoint.position, heavyAttackRadius);
        }

    }
}