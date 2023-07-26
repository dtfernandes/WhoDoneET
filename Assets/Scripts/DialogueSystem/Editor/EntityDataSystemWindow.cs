using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

namespace DialogueSystem.Editor
{
    public class EntityDataSystemWindow : EditorWindow 
    {

        EntityData data;
        SerializedObject serObj;
        // The variable to control where the scrollview 
        //'looks' into its child elements.
        Vector2 scrollPosition;
        private string searchKeyword = ""; // Variable to store the search keyword


        /// <summary>
        /// Method responsible for opening the EntityDataSystem Window 
        /// </summary>
        [MenuItem("Dialogue/Entity Data")]
        public static void OpenEntityDataWindow()
        {
            EntityDataSystemWindow window = GetWindow<EntityDataSystemWindow>();
            window.titleContent = new GUIContent(text: "Entity Data");
            window.data = GetEntityData();
            EditorUtility.SetDirty(window.data);
            DisplayData(window.data);
        }


        private void OnGUI()
        {

            GUILayout.Space(10);

            if (GUILayout.Button("Add New Preset"))
            {
                data.AddNewPreset();
            }

            GUILayout.Space(10);

            // Region: Search Bar
            #region Search Bar
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search: ");
            searchKeyword = GUILayout.TextField(searchKeyword);
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(20);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < data.data.Count; i++)
            {

                if (searchKeyword != "")
                {
                    if (data.data[i].EntityName == "")
                    {
                        continue;
                    }
                    // Skip presets that do not match the search keyword
                    if (!(data?.data[i]?.EntityName?.ToLower()?.Contains(searchKeyword.ToLower()) ?? true))
                    {
                        continue;
                    }
                }
                

                EntityInfo info = data.data[i];

                DrawUILine(new Color(0.1f, 0.1f, 0.1f, 1), 1, 0);

                // Create a custom GUIStyle with the desired background color
                GUIStyle customButtonStyle = new GUIStyle(GUI.skin.label);
                customButtonStyle.normal.background = MakeTexture(1, 1, new Color(0.4f, 0.4f, 0.4f)); // Set the background color here
                customButtonStyle.margin = new RectOffset(0,0,0,0);

                string presetName = (info?.EntityName?.Trim() ?? "") == "" ? $"Preset {i}" : info.EntityName;

                if (GUILayout.Button(presetName, customButtonStyle))
                {
                    data.data[i].Hidden = !data.data[i].Hidden;
                }



                // Hidable Content
                if (data.data[i].Hidden)
                {
                    GUIStyle contentStyle = new GUIStyle(GUI.skin.label);
                    contentStyle.normal.background = MakeTexture(1, 1, new Color(0.3f, 0.3f, 0.3f)); // Set the background color here
                    contentStyle.margin = new RectOffset(0, 0, 0, 0);

                    GUILayout.BeginVertical(contentStyle);

                    #region Entity Content
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal(GUILayout.Height(100));
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();

                    // Region: Entity Name
                    #region Entity Name
                    info.EntityName = EditorGUILayout.TextField(info.EntityName,
                        GUILayout.Width(Math.Min(300, position.width - 120)));
                    #endregion

                    GUILayout.FlexibleSpace();

                    // Region: Buttons
                    #region Buttons
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button($"Open Settings"))
                    {
                        ExpressionWindow.ShowWindow(info.Expressions);
                    }

                    GUIStyle redButtonStyle = new GUIStyle(GUI.skin.button);
                    
                    redButtonStyle.normal.textColor = Color.white; // Set text color to white
                    redButtonStyle.fixedWidth = position.width * 0.1f;
                    redButtonStyle.alignment = TextAnchor.MiddleCenter; // Center the text in the button

                    if (GUILayout.Button($"X", redButtonStyle))
                    {
                        bool confirmed = EditorUtility.DisplayDialog("Confirm Removal", "Are you sure you want to remove the preset?", "Yes", "No");
                        if (confirmed)
                        {
                            // Remove the preset when "Yes" is clicked
                            data.RemovePresetAt(i);
                        }
                    }

                    GUILayout.EndHorizontal();
                    #endregion

                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();

                    // Region: Entity Sprite
                    #region Entity Sprite
                    GUILayout.BeginVertical();
                    info.Expressions =
                       (ExpressionPreset)EditorGUILayout.ObjectField(obj: info.Expressions,
                       objType: typeof(ExpressionPreset), false,
                       GUILayout.Height(20), GUILayout.Width(100));

                    Expression defaultExpression = info.Expressions?.Emotions?[0] ?? null;
                    Sprite defaultSprite = null;
                    if (defaultExpression != default)
                    {
                        defaultSprite = defaultExpression.Image;
                    }
                  
                    EditorGUILayout.ObjectField(obj: defaultSprite,
                        objType: typeof(Sprite), false,
                        GUILayout.Height(100), GUILayout.Width(100));
                    GUILayout.EndVertical();
                    #endregion

                    GUILayout.Space(10);

                    GUILayout.EndHorizontal();
                    #endregion

                    GUILayout.Space(5);
                    GUILayout.EndVertical();
                }
            }


            GUILayout.EndScrollView();
        }

        private static void DisplayData(EntityData data)
        {
            
        }

        private static EntityData GetEntityData()
        {
           
            //AssetDatabase.DeleteAsset("Assets/DialogueSystem/Editor/Resources/EntityData.asset");           
            EntityData[] foundObjects = Resources.LoadAll<EntityData>("");

            bool dataExists = foundObjects.Length > 0;
            
            if (!dataExists)
            {
                Debug.Log("Curious");
                EntityData asset = ScriptableObject.CreateInstance<EntityData>();
                AssetDatabase.CreateAsset(asset,
                    "Assets/DialogueSystem/Editor/Resources/EntityData.asset");
            }
            
            return Resources.Load<EntityData>("EntityData");
        }

        //alexanderamey
        //https://forum.unity.com/threads/horizontal-line-in-editor-window.520812/
        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }


        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }
    }
  
}
