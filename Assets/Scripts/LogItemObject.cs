using UnityEngine;
using TMPro;

public class LogItemObject: MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    public LogItem Item {get; private set; }

    public void Display(string logText, LogItem lItem)
    {
        _text.text = logText;
        Item = lItem;
    }
}
