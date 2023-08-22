using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterLogPageDisplay : LogPage
{
 
    [SerializeField]
    private Image _logImage, _shadowImage;
    [SerializeField]
    private TextMeshProUGUI _logName;

    [SerializeField]
    private EntiyDisplayData[] _displayData;

    void OnEnable()
    {
        if(entity != null)
            Display(entity ?? 0);
    }

    public override void Display(LogEntity entity)
    {
        base.Display(entity);

         EntiyDisplayData data = _displayData[((int)entity) - 1];

        _logImage.sprite = data.Image;
        _shadowImage.sprite = data.Image;
        _logName.text = data.Name;
    }
}
