using UnityEngine;

/// <summary>
/// Class that handles the diplay of the Log data
/// </summary>
public class LogDisplay : MonoBehaviour
{

    private InvestigationLog _log;

    [SerializeField]
    public CharacterLogPageDisplay _characterPage;

    [SerializeField]
    public GameObject _mainPage;


    // Start is called before the first frame update
    void Start()
    {
        _log = GameSettings.Instance.Log;
    }

    public void SelectPage(int characterIndex)
    {

        if(characterIndex < 0) return;

        _mainPage.SetActive(false);
        _characterPage.gameObject.SetActive(true);
        _characterPage.Display((LogEntity)(characterIndex + 1));
    }

    public void GoBack(bool isCharacter)
    {
        if(isCharacter)
        {
            _characterPage.gameObject.SetActive(false);
        }

        
        _mainPage.SetActive(true);
    }

}
