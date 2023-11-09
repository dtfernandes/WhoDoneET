using UnityEngine;
using TMPro;
using System.Linq;

public class SolvitMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _people, _weapons, _motive;

    private GameSettings _settings;

    void Awake()
    {
        _settings = GameSettings.Instance;
    }

    public void Setup()
    {
        gameObject.SetActive(true);

        _people.options = 
            _settings.Peoples.Select(x => new TMP_Dropdown.OptionData(x)).ToList();
        _weapons.options = 
            _settings.Weapons.Select(x => new TMP_Dropdown.OptionData(x)).ToList();
        _motive.options = 
            _settings.Motives.Select(x => new TMP_Dropdown.OptionData(x)).ToList();    
    }

    public void Submit()
    {
        string p = _people.options[_people.value].text;
        string w = _weapons.options[_weapons.value].text;
        string m = _motive.options[_motive.value].text;

        _settings.Submit(p, w, m);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameResult");
    }
}
