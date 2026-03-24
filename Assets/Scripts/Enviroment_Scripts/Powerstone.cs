using UnityEngine;
using UnityEngine.SceneManagement;
public class Powerstone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что в триггер зашел именно игрок
        if (collision.CompareTag("Player"))
        {
            // Пытаемся найти скрипт Player на объекте, который вошел в триггер
            Player playerScript = collision.GetComponent<Player>();

            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
}
