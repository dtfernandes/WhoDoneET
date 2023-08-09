using UnityEngine;
using UnityEditor;

namespace DialogueSystem
{
    public class AddToLogEvent : CustomFunction
    {
        private int _entitySelected;
        private string _text;

        // Define a custom GUIStyle without any padding or margin
        private GUIStyle miniButton;



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

        }


        public void UpdateNode()
        {

        }
    }
}