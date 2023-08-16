using UnityEngine;

[System.Serializable]
public struct LogItem
{

    

    public LogItem(string logText, LogEntity entity, string guid)
    {
        GUID = guid;
        LogText = logText;
        Entity = entity;
    }

    public string GUID { get; private set; }

    [field:SerializeField]
    public string LogText { get; private set; }
     [field:SerializeField]
    public LogEntity Entity { get; private set; }
}
