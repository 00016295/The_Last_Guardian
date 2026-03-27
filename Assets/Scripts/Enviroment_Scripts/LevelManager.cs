using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    public Button[] buttons;
    public int levelindex = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelindex = PlayerPrefs.GetInt("Level", levelindex);
    }
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
    // Update is called once per frame
    void Update()
    {

        //if (Input.GetMouseButtonDown(1))
        //{
        //    PlayerPrefs.SetInt("LevelAt", levelindex + 1);
        //}

        //for (int i = 0; i < buttons.Length; i++)
        //{
        //    if (levelindex > i)
        //    {
        //        buttons[i].interactable = true;
        //    }
        //    else
        //    {
        //        buttons[i].interactable = false;
        //    }
        //}
    }
}
