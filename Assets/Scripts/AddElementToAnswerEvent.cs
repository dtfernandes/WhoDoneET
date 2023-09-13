using UnityEngine;
using UnityEditor;
using DialogueSystem;
using System;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class AddElementToAnswerEvent : CustomFunction
{
    [SerializeField]
    private AnswerType _answerType;
    [SerializeField]
    private string _text;

    // Define a custom GUIStyle without any padding or margin
    private GUIStyle miniButton;


    #if UNITY_EDITOR

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
        string[] entities = Enum.GetValues(typeof(AnswerType)).Cast<AnswerType>().Select(x => x.ToString()).ToArray();

        GUILayout.Label("Add new Answer Event");

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Space(-15);
        _answerType = (AnswerType)(EditorGUILayout.Popup((int)_answerType, entities));

        if (GUILayout.Button("X", miniButton))
        {

        }

        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        string prevText = _text;
        _text = GUILayout.TextField(_text, GUILayout.Height(25));

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

#endif

    public override void Invoke()
    {
        GameSettings gameSettings = GameSettings.Instance;

        List<string> answerList = new List<string> { };

        switch(_answerType)
        {
            case AnswerType.Who:
                answerList = gameSettings.Peoples;
                break;
            case AnswerType.Why:
                answerList = gameSettings.Motives;
                break;
            case AnswerType.WithWhat:
                answerList = gameSettings.Weapons;
                break;
        }

        bool added = answerList.Contains(_text);

        if(!added)
        {
            answerList.Add(_text);
        }
    }

    public void UpdateNode()
    {

    }
}

public enum AnswerType
{
    Who,
    WithWhat,
    Why
}