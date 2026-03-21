using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 15; // Maximum health of the enemy
    public float attackCooldown = 2f;
    private float nextAttackTime = 1f;
    private bool isDead = false;

    public float patrolSpeed = 2.2f; // Speed at which the enemy patrols
    public Transform detectPoint; // Point from which the enemy detects the player
    public float distance = .3f; // Distance for player detection
    public LayerMask whatIsGround; // Layer mask to identify ground

    public Transform player; 
    public float chaseSpeed = 3.5f; // Speed at which the enemy chases the player
    public float retrieveDistance = 1.5f; // Distance at which the enemy retrieves the player

    private Animator animator;
    private bool facingLeft; // Direction of enemy movement
    private bool isPlayerInAttackRange; // Flag to check if player is in attack range

    public Transform attackPoint; // Point from which the enemy attacks
    public float attackRadius = 1f; // Range of the enemy's attack
    public LayerMask whatIsPlayer; // Layer mask to identify the player




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        facingLeft = true; // Initialize enemy to face left
        isPlayerInAttackRange = false; // Initialize player attack range flag
        animator = this.gameObject.GetComponent<Animator>(); // Get the Animator component attached to the enemy
    }

    // Update is called once per frame
    void Update()
    {
        if (maxHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
            return; // Выходим из Update, так как враг мертв
        }
        if (isDead) return;

        Flip();

        if(isPlayerInAttackRange == true)
        {
            if (player.position.x > transform.position.x && facingLeft == true)
            {
                transform.eulerAngles = new Vector3 (0f,-180f,0f); // Flip the enemy to face right
                facingLeft = false; // Update facing direction
            }
            else if(player.position.x < transform.position.x && facingLeft == false)
            { 
                transform.eulerAngles = new Vector3(0f, 0f, 0f); // Flip the enemy to face left
                facingLeft = true; // Update facing direction
            }
            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, player.position) < retrieveDistance)
            {
                if (Time.time >= nextAttackTime)
            {
                animator.SetBool("Attack", true);
                // Время следующей атаки обновится, когда сработает Animation Event или метод Attack
            }
            }
        }
        else
        {
            Patrol();
            animator.SetBool("Attack", false);
              
        }

        
    }
    void Flip()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(detectPoint.position, Vector2.down, distance, whatIsGround);

        if (hitInfo == false)
        {
            if (facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 180f, 0); // Flip the enemy to face right
                facingLeft = false; // Update facing direction

            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0); // Flip the enemy to face left
                facingLeft = true; // Update facing direction
            }
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.left * patrolSpeed * Time.deltaTime); // Move the enemy left based on patrol speed
    }

    public void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;

        Collider2D collinfo = Physics2D.OverlapCircle(attackPoint.position,attackRadius,whatIsPlayer);

        if (collinfo)
        {
            if(collinfo.gameObject.GetComponent<Player>() != null)
            {
                collinfo.gameObject.GetComponent<Player>().TakeDamage(5); // Log message for debugging purposes
            }
        }
    }

    public void TakeDamage(int damage)
    {
        maxHealth -= damage; // Reduce the enemy's health by the damage amount
        animator.SetTrigger("damage"); // Trigger the "Hurt" animation in the Animator
        if (maxHealth <= 0)
        {
            return; // Call the Die method if health is zero or less
        }
    }


    // Method called when another collider enters the trigger collider attached to the enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInAttackRange = true; 
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInAttackRange = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint != null)
        {
            Gizmos.color = Color.yellow; // Set gizmo color to red
            Gizmos.DrawRay(detectPoint.position, Vector2.down * distance); // Draw a line to visualize the detection range
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.red; // Set gizmo color to red
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius); // Draw a wire sphere to visualize the attack range
        }
    }
    void Die()
    {
        
        

        // Запускаем корутину
        StartCoroutine(ExecuteDeath());
    }

    IEnumerator ExecuteDeath()
    {
        Debug.Log("Враг умирает..."); //

        
        animator.SetBool("isDead",true);

        // 2. Ждем 3 секунды
        yield return new WaitForSeconds(3f);

        // 3. Удаляем объект
        Destroy(gameObject); //
    }
}
