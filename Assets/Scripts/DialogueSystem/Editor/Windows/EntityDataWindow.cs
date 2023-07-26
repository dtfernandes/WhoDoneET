using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Wnidow that display the dialogue entity data used in this project
    /// </summary>
    public class EntityDataWindow : EditorWindow
    {

        //List of entities
        private EntityData _entityData;
        // The variable to control where the scrollview 
        //'looks' into its child elements.
        private Vector2 _scrollPosition;
        // Variable to store the search keyword
        private string _searchKeyword = "";

        [MenuItem("Dialogue/Entity Data Test")]
        public static void ShowExample()
        {
            EntityDataWindow window = GetWindow<EntityDataWindow>();

            window.titleContent = new GUIContent(text: "Entity Data");

            //Get entity 
            window._entityData = GetEntityData();

            EditorUtility.SetDirty(window._entityData);
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);
       
            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/DialogueSystem/Editor/Windows/EntityDataWindow.uss");
            VisualElement labelWithStyle = new Label("Hello World! With Style");
            labelWithStyle.styleSheets.Add(styleSheet);
            root.Add(labelWithStyle);
        }

        private static EntityData GetEntityData()
        {
            EntityData[] foundObjects = Resources.LoadAll<EntityData>("");

            bool dataExists = foundObjects.Length > 0;

            if (!dataExists)
            {
                EntityData asset = ScriptableObject.CreateInstance<EntityData>();
                AssetDatabase.CreateAsset(asset,
                    "Assets/DialogueSystem/Editor/Resources/EntityData.asset");
            }

            return Resources.Load<EntityData>("EntityData");
        }

    }
}