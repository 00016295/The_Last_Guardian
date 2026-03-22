using UnityEngine;

public class LootItem : MonoBehaviour
{
    public int scoreValue = 10;
    // Для триггеров (Is Trigger ВКЛ)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryCollect(collision.gameObject);
    }

    // Для обычных столкновений (Is Trigger ВЫКЛ)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryCollect(collision.gameObject);
    }

    private void TryCollect(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(scoreValue);
            }
            Debug.Log("Монета подобрана!");
            Destroy(gameObject);
        }
    }
}