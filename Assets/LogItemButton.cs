using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(LogItemObject))]
public class LogItemButton : MonoBehaviour
{

    private LogItemObject _logItem;
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
    }

    
}
