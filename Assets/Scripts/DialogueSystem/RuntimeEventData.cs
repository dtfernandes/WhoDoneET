using UnityEngine;

[System.Serializable]
public class RuntimeEventData
{
    /// <summary>
    /// Constructor for the RuntimeEventData class
    /// </summary>
    /// <param name="classType">Type of the class this event is from</param>
    /// <param name="methodInfo">Method inside the type</param>
    /// <param name="index">Letter this event should be played in</param>
    public RuntimeEventData(System.Type classType, string methodName, int index, object[] parameters = null)
    {
        ClassType = classType;
        MethodName = methodName;
        TriggerIndex = index;
        Params = parameters;
    }

    [field: SerializeField]
    public System.Type ClassType { get; private set; }

    [field: SerializeField]
    public string MethodName{ get; private set; }

    [field: SerializeField]
    public int TriggerIndex { get; private set; }

    public object[] Params { get; private set; }

    #region  Editor

    [HideInInspector] [field: SerializeField]
    public int ClassIndex { get; set; }

    [HideInInspector] [field: SerializeField]
    public int MethodIndex { get; set; }

    [HideInInspector] [field: SerializeField]
    public bool ShowText { get; set; }
    
    [HideInInspector] [field: SerializeField]
    public bool ShowSelf { get; set; }
    
    #endregion
}
