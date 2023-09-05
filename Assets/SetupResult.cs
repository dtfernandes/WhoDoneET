using UnityEngine;

public class SetupResult : MonoBehaviour
{
    private int _result;

    const string _people = "Office Lady";
    const string _gun = "Gun";
    const string _motive = "Dispote Over Scam";

    private GameSettings _settings;

    [SerializeField]
    private UnityEngine.UI.Text _bigTitle, _firstPart, _secondPart, _thirdPart;

    // Start is called before the first frame update
    void Start()
    {
        _settings = GameSettings.Instance;
        
        _result = 0;

        if( _settings.Selected[0] == _people )
        {
            _result += 1;
        }

         if( _settings.Selected[1] == _gun )
        {
            _result += 1;
        }

         if( _settings.Selected[2] == _motive )
        {
            _result += 1;
        }


        _firstPart.text = $"This saturday the 25th, {_settings.Selected[0]} was trialed for the murder of Zé dos Sacos."
        + " The trial had the duration of 30 min and it became clear the prosecution had little evidence to work with. ";

        _secondPart.text = $"The chief of police Nelio Codices had this to say: "
        + "'I deeply apologize for the incompetence of my subordinates and take full responsibility. "
        + "I will do everything in my power to make sure we only work with the best from now on'";

        _thirdPart.text = $"The lead detective of this investigation was promptly fired after the fact and "
        + "is now taking imense backlash in the new hit social media Y.";
    }

}
