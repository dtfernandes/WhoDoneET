using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomLogPageDisplay : MonoBehaviour
{
    //Prefab element that represents a line in the log
    [SerializeField]
    private LogItemObject _logItemPREFAB;

    private InvestigationLog _log;

    [SerializeField]
    private GameObject _logLayout;

    public void Display()
    {

        if (_log == null)
            _log = GameSettings.Instance.Log;

        //Clear Layout
        foreach (Transform t in _logLayout.transform)
        {
            Destroy(t.gameObject);
        }

        if (_log.Items == null) Debug.LogWarning("Log is null. This shouldn't happen");
        if (_log.Items.Count <= 0) return;

        List<LogItem> selectedItem = _log.Items.Where(x => x.Entity == LogEntity.Room).ToList();

        foreach (LogItem lItem in selectedItem)
        {
            LogItemObject item = Instantiate(_logItemPREFAB, transform.position, _logLayout.transform.rotation, _logLayout.transform);

            item.Display(lItem.LogText);
        }
    }

}