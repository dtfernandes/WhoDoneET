using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private EventSystem eSysteM;
    [SerializeField]
    private GameObject Yes;

    public void OpenMenu()
    {
        eSysteM.SetSelectedGameObject(null);
        eSysteM.SetSelectedGameObject(Yes);
        Debug.Log("hm");
    }

    public void GoToGame()
    {

        SceneManager.LoadScene("TestTiles 2");
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
