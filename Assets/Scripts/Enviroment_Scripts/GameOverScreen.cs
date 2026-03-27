using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverUI;

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); 
    }
}
