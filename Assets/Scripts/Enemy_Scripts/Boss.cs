using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boss : MonoBehaviour
{
    // Health Settings
    public int maxHealth = 15;
    public GameObject healthBarUI; 
    private bool isDead = false;
    public Slider healthBar;


    // Attack Settings
    public float attackCooldown = 5f;
    private float nextAttackTime = 1f;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask whatIsPlayer;

    // Movement Settings
    public float chaseSpeed = 3.5f;
    public float retrieveDistance = 1.5f;
    public Transform detectPoint;
    public float distance = .3f;
    public LayerMask whatIsGround;
    public Transform player;

    private Animator animator;
    private bool facingLeft = true;
    private bool isPlayerInAttackRange = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (healthBarUI != null) healthBarUI.SetActive(false); // Прячем UI в начале
    }

    void Update()
    {
        if (maxHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
            return;
        }
        if (isDead) return;

        Flip();

        if (isPlayerInAttackRange == true)
        {
            // Логика поворота к игроку
            HandleFacingDirection();

            float distToPlayer = Vector2.Distance(transform.position, player.position);

            if (distToPlayer > retrieveDistance)
            {
                // Преследование
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
                animator.SetFloat("Run", 1f); // Если есть анимация бега
            }
            else
            {
                // Атака
                animator.SetFloat("Run", 0f);
                if (Time.time >= nextAttackTime)
                {
                    PerformRandomAttack();
                }
            }
        }
        
    }

    void PerformRandomAttack()
    {
        int randomAttack = Random.Range(1, 4); // Выберет 1, 2 или 3
        animator.SetTrigger("Attack_" + randomAttack);

        nextAttackTime = Time.time + attackCooldown;
    }

    // Метод вызывается из анимации через Animation Event (событие "Attack")
    public void Attack()
    {
        Collider2D collinfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsPlayer);
        if (collinfo && collinfo.GetComponent<Player>() != null)
        {
            collinfo.GetComponent<Player>().TakeDamage(5);
        }
    }

    void HandleFacingDirection()
    {
        if (player.position.x > transform.position.x && facingLeft)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingLeft = false;
        }
        else if (player.position.x < transform.position.x && !facingLeft)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingLeft = true;
        }
    }

    void Flip() // Проверка края платформы
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(detectPoint.position, Vector2.down, distance, whatIsGround);
        if (hitInfo.collider == null)
        {
            facingLeft = !facingLeft;
            float angle = facingLeft ? 0 : 180;
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }

    

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        maxHealth -= damage;
        healthBar.value = maxHealth;
        animator.SetTrigger("Damage"); // Совпадает с твоим параметром "Damage"
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInAttackRange = true;
            if (healthBarUI != null) healthBarUI.SetActive(true); // Показываем здоровье
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInAttackRange = false;
            if (healthBarUI != null) healthBarUI.SetActive(false); // Прячем здоровье
        }
    }

    void Die()
    {
        if (healthBarUI != null) healthBarUI.SetActive(false);
        StartCoroutine(ExecuteDeath());
    }

    IEnumerator ExecuteDeath()
    {
        animator.SetBool("Is_Dead", true); // Твой параметр "Is_Dead"
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint != null) Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);
        if (attackPoint != null) Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}