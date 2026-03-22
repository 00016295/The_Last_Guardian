using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [Header("Настройки лута")]
    public GameObject lootPrefab; 
    public int chance = 100;      

    // Метод, который вызывается при смерти врага
    public void DropLoot()
    {
        // Цикл выполнится 10 раз
        for (int i = 0; i < 10; i++)
        {
            // Создаем объект
            GameObject droppedItem = Instantiate(lootPrefab, transform.position, Quaternion.identity);

            // Разбрасываем их в разные стороны, чтобы они не слиплись
            Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                
                Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                rb.AddForce(randomDirection * Random.Range(3f, 6f), ForceMode2D.Impulse);
            }
        }
    }
}
