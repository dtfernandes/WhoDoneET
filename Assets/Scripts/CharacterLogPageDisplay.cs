using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterLogPageDisplay : MonoBehaviour
{
    //Prefab element that represents a line in the log
    [SerializeField]
    private LogItemObject _logItemPREFAB;

    private InvestigationLog _log;

    [SerializeField]
    private GameObject _logLayout;

    private LogEntity? _entity;

    [SerializeField]
    private EntiyDisplayData[] _displayData;


    [SerializeField]
    private Image _logImage, _shadowImage;
    [SerializeField]
    private TextMeshProUGUI _logName;

    void OnEnable()
    {
        if(_entity != null)
            Display(_entity ?? 0);
    }

    public void Display(LogEntity entity)
    {
        _entity = entity;

        if(_log == null)
            _log = GameSettings.Instance.Log;

        EntiyDisplayData data = _displayData[((int)entity) - 1];

        _logImage.sprite = data.Image;
        _shadowImage.sprite = data.Image;
        _logName.text = data.Name;

        //Clear Layout
        foreach(Transform t in _logLayout.transform)
        {
            Destroy(t.gameObject);
        }

        if(_log.Items == null) Debug.LogWarning("Log is null. This shouldn't happen");
        if(_log.Items.Count <= 0) return;

        List<LogItem> selectedItem = _log.Items.Where(x => x.Entity == _entity).ToList();

        foreach(LogItem lItem in selectedItem)
        {
            LogItemObject item = Instantiate(_logItemPREFAB, transform.position, _logLayout.transform.rotation, _logLayout.transform);

            item.Display(lItem.LogText);
        }
    }
}
