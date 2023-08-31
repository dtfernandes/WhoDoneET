using UnityEditor;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueEdgeInspector))]
    public class DialogueEdgeInspectorEditor : UnityEditor.Editor
    {
        private DialogueEdgeInspector _inspector;
        private SerializedProperty _choiceData, _hideIds;


        public void OnEnable()
        {
            _inspector = target as DialogueEdgeInspector;
            _choiceData = serializedObject.FindProperty("_choiceData");
            _hideIds = _choiceData.FindPropertyRelative("_hideIDs");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_hideIds);

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(target);
        }
    }
}
