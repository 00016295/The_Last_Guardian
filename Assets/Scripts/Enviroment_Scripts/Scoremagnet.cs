using UnityEngine;

public class Scoremagnet : MonoBehaviour
{
    public float magnetRadius = 5f;  // Дистанция, с которой монета "заметит" игрока
    public float moveSpeed = 10f;    // Скорость полета к игроку

    public BoxCollider2D triggerCollider;  // Тот, что с галочкой Is Trigger
    public CircleCollider2D physicalCollider;

    private Transform _playerTransform;
    private bool _isAttracted = false;
        private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Ищем игрока по тегу (убедитесь, что у игрока стоит тег "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        // Простая проверка безопасности
        if (triggerCollider == null || physicalCollider == null)
        {
            Debug.LogError($"На монете {gameObject.name} не заполнены ссылки на коллайдеры в скрипте CoinMagnet!", gameObject);
        }
    }

    void Update()
    {
        if (_playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= magnetRadius)
        {
            _isAttracted = true;
        }

        if (_isAttracted)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                _playerTransform.position,
                moveSpeed * Time.deltaTime
            );

            // --- НОВЫЙ БЛОК: Безотказный подбор ---
            if (distanceToPlayer < 0.3f) // Если монета почти коснулась центра игрока
            {
                CollectCoin();
            }
        }
    }

    void CollectCoin()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(10);
        }
        Debug.Log("Монета подобрана через дистанцию!");
        Destroy(gameObject);
    }
}
