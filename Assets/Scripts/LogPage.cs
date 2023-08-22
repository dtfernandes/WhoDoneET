using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class LogPage: MonoBehaviour
{
    //Prefab element that represents a line in the log
    [SerializeField]
    private LogItemObject _logItemPREFAB;
    
    [SerializeField]
    private GameObject _logLayout;

    private InvestigationLog _log;
    protected LogEntity? entity;

    public virtual void Display(LogEntity entity)
    {
        this.entity = entity;

        if(_log == null)
            _log = GameSettings.Instance.Log;
      
        //Clear Layout
        foreach(Transform t in _logLayout.transform)
        {
            Destroy(t.gameObject);
        }

        if(_log.Items == null) Debug.LogWarning("Log is null. This shouldn't happen");
        if(_log.Items.Count <= 0) return;

        List<LogItem> selectedItem = _log.Items.Where(x => x.Entity == entity).ToList();

        foreach(LogItem lItem in selectedItem)
        {
            LogItemObject item = Instantiate(_logItemPREFAB, transform.position, _logLayout.transform.rotation, _logLayout.transform);

            item.Display(lItem.LogText, lItem);
        }
    }
}