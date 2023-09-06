using UnityEngine;

/// <summary>
/// Class that handles the diplay of the Log data
/// </summary>
public class LogDisplay : MonoBehaviour
{

    private InvestigationLog _log;

    [SerializeField]
    private LogPage _characterPage;

    [SerializeField]
    private SolvitMenu _solveItMenu;

    [SerializeField]
    public GameObject _mainPage;


    // Start is called before the first frame update
    void Start()
    {
        _log = GameSettings.Instance.Log;
    }

    public void SelectPage(int characterIndex)
    {
        _characterPage.gameObject.SetActive(false);
        _solveItMenu.gameObject.SetActive(false);

        if(characterIndex < 0) return;

        _mainPage.SetActive(false);
        _characterPage.gameObject.SetActive(true);
        Debug.Log(((LogEntity)characterIndex).ToString());
        _characterPage.Display((LogEntity)characterIndex);
    }

    public void GoBack()
    {
        _characterPage.gameObject.SetActive(false);
        _solveItMenu.gameObject.SetActive(false);
        _mainPage.SetActive(true);
    }

}
