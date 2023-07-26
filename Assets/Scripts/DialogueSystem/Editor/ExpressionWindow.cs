using DialogueSystem;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class ExpressionWindow : EditorWindow
{
    private static EntityData _entityData;
    private static ExpressionPreset _expressionPreset;

    [MenuItem("Window/Expression Window")] // This will create a new menu item under "Window"
    public static void ShowWindow(ExpressionPreset preset)
    {
        _expressionPreset = preset;
        // Get existing open window or if none, make a new one:
        ExpressionWindow window = (ExpressionWindow)EditorWindow.GetWindow(typeof(ExpressionWindow));
        window.titleContent = new GUIContent("Expression Preset");
        EditorUtility.SetDirty(preset);


        window.Show();
    }

    private void OnGUI()
    {
        if (_expressionPreset == null)
        {
            EditorGUILayout.HelpBox("Please assign an Expression Preset to display.", MessageType.Info);
          
            if (GUILayout.Button("Create new Empty Preset"))
            {
               
            }

            return;
        }

        if (GUILayout.Button("Add New Expression"))
        {
            _expressionPreset.AddNewExpression();
        }


        if (_expressionPreset.Emotions == null)
        {
            return;
        }

        if (_expressionPreset.Emotions.Count <= 0)
        {
            return;
        }

        GUILayout.Label("Expression Preset:", EditorStyles.boldLabel);

        // Display emotions in grid
        DisplayEmotionsGrid();
    }

    private void DisplayEmotionsGrid()
    {
        ExpressionWindow window = (ExpressionWindow)EditorWindow.GetWindow(typeof(ExpressionWindow));

        int width = (int)window.position.width;
        int height = (int)window.position.height;
        GUILayout.BeginVertical();

        int columnCount = 4; // Number of emotions per row

        for (int i = 0; i < _expressionPreset.Emotions.Count; i += columnCount)
        {
 
            GUILayout.BeginHorizontal();

            for (int j = 0; j < columnCount && i + j < _expressionPreset.Emotions.Count; j++)
            {
                GUIStyle contentStyle = new GUIStyle();
                contentStyle.fixedWidth = (width / 4);


                Expression emotion = _expressionPreset.Emotions[i + j];

                GUILayout.BeginVertical(contentStyle);
               
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                emotion.Image =
                    (Sprite)EditorGUILayout.ObjectField(obj: emotion.Image,
                    objType: typeof(Sprite), false,
                    GUILayout.Height(100), GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                emotion.EmotionName = EditorGUILayout.TextField(emotion.EmotionName, GUILayout.ExpandWidth(true));
                // Add more display options for each emotion if needed

                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }

}
