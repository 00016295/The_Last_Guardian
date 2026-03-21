using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        // Quit the application
        Application.Quit();
    }
}
