using UnityEngine;
using UnityEngine.SceneManagement;
public class Powerstone : MonoBehaviour
{
    public LevelComplete resultsUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 1;
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
