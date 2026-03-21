using UnityEngine;

public class Healing : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что в триггер зашел именно игрок
        if (collision.CompareTag("Player"))
        {
            // Пытаемся найти скрипт Player на объекте, который вошел в триггер
            Player playerScript = collision.GetComponent<Player>();

            if (playerScript != null)
            {
                // Вызываем метод лечения (мы добавим его ниже в скрипт игрока)
                playerScript.Heal(healAmount);

                // Удаляем аптечку со сцены
                Destroy(gameObject);
            }
        }
    }
}
