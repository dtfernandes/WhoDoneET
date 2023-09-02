using UnityEngine;
using UnityEditor;
using DialogueSystem;
using System;
using System.Linq;

[System.Serializable]
public class AddToLogEvent : CustomFunction
{
    [SerializeField]
    private LogEntity _entitySelected;
     [SerializeField]
    private string _text;

    // Define a custom GUIStyle without any padding or margin
    private GUIStyle miniButton;


    [ContextMenu("ShowContextMenu")]
    private void ShowContextMenu()
    {
        // Create and show your context menu options here
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Copy"), false, UpdateID);
        menu.ShowAsContext();
    }

    private void UpdateID()
    {
        // Handle Option 1 action here
        Debug.Log(GUID);
        GUIUtility.systemCopyBuffer = GUID;
    }


    public override void OnEnable()
    {
        // Initialize the custom GUIStyle
        miniButton = new GUIStyle(EditorStyles.miniButton);
    }

    public override void Draw()
    {
        string[] entities = Enum.GetValues(typeof(LogEntity)).Cast<LogEntity>().Select(x => x.ToString()).ToArray();

        GUILayout.Label("Add to Log Event");

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Space(-15);
        _entitySelected = (LogEntity)(EditorGUILayout.Popup((int)_entitySelected, entities));

        if (GUILayout.Button("X", miniButton))
        {

        }

        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        string prevText = _text;
        _text = GUILayout.TextField(_text, GUILayout.Height(100));

        if (_text != prevText)
        {
            onUpdate?.Invoke();
        }

        //Dumb
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = Color.grey;



        //Even dumber
        Rect labelRect = GUILayoutUtility.GetRect(new GUIContent("ID: " + GUID), style);

        if (Event.current.type == EventType.ContextClick && labelRect.Contains(Event.current.mousePosition))
        {
            ShowContextMenu();
            Event.current.Use();
        }
        GUILayout.Space(-5);
        GUILayout.Label("ID: " + GUID, style);

        GUILayout.Space(5);
    }

    public override void Invoke()
    {
        LogItem item = new LogItem(_text, _entitySelected, GUID);
        GameSettings.Instance.Log.AddItem(item);
    }

    public void UpdateNode()
    {

    }
}
