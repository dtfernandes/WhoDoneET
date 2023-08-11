using UnityEngine;
using UnityEditor;

namespace DialogueSystem
{
    [System.Serializable]
    public class AddToLogEvent : CustomFunction
    {
        private int _entitySelected;
        private string _text;

        // Define a custom GUIStyle without any padding or margin
        private GUIStyle miniButton;


        [ContextMenu("ShowContextMenu")]
        private void ShowContextMenu()
        {
            // Create and show your context menu options here
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Update ID"), false, UpdateID);
            menu.ShowAsContext();
        }

        private void UpdateID()
        {
            // Handle Option 1 action here
            Debug.Log("Option 1 selected");
        }


        public override void OnEnable()
        {
            // Initialize the custom GUIStyle
            miniButton = new GUIStyle(EditorStyles.miniButton);
        }

        public override void Draw()
        {
            string[] entities = new string[] { "Lizard Guy", "Cop", "Cup", "Other Shit" };

            GUILayout.Label("Add to Log Event");

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(-15);
            _entitySelected = EditorGUILayout.Popup(_entitySelected, entities);

            if (GUILayout.Button("X", miniButton))
            {

            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            string prevText = _text;
            _text = GUILayout.TextField(_text, GUILayout.Height(100));

            if(_text != prevText)
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
            Debug.Log("This is to add: "  +_text);
        }

        public void UpdateNode()
        {

        }
    }
}