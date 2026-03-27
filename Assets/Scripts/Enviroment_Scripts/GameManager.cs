using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    public void SetupGameOver()
    {
        gameOverScreen.SetActive(true); 
        Time.timeScale = 0f;       
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

   
    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
