using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("TestTiles2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
