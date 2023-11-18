using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Method that handles a button related to a log item
/// </summary>
[RequireComponent(typeof(Button), typeof(LogItemObject))]
public class LogItemButton : MonoBehaviour
{
    // Log item represented by the button
    private LogItemObject _logItem;

    //Button component
    private Button _button;

    void Awake()
    {
        _logItem = GetComponent<LogItemObject>();
        _button = GetComponent<Button>();

        _button.onClick.AddListener(SelectButton);
    }

    // Start is called before the first frame update
    void SelectButton()
    {
        GameSettings.Instance.DialogueHandler.SpecialChoice(_logItem.Item.GUID, SpecialChoice.Skip);
        MiniLog.Deactivate();
    }

    
}
