using UnityEngine;
using TMPro;

public class LogItemObject: MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    public void Display(string logText)
    {
        _text.text = logText;
    }
}
