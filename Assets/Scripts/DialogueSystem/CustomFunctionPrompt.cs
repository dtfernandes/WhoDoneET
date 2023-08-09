using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;

namespace DialogueSystem
{
    public class CustomFunctionPrompt : CustomFunction
    {
        private int selectedClassIndex = 0;
        private CustomFunction func;

        public CustomFunctionPrompt()
        {
        }

        public CustomFunctionPrompt(CustomFunction func)
        {
            this.func = func;
        }

        public Action<CustomFunction> onSelectFunction { get; set; }

        public override void Draw()
        {

            // Create an array to store the names of the inheriting classes
            List<string> classNames =
                FindInheritedClasses<CustomFunction>().Select(t => t.Name).ToList();

            classNames.Insert(0, "None");

            int o = selectedClassIndex;

            // Display the dropdown menu with the class names
            selectedClassIndex = EditorGUILayout.Popup(selectedClassIndex, classNames.ToArray(), GUILayout.ExpandWidth(true));

            if (o != selectedClassIndex)
            {
                //There was a change

                CustomFunction customFunc = this;

                switch (classNames[selectedClassIndex])
                {
                    case nameof(AddToLogEvent):
                        customFunc = new AddToLogEvent();
                        break;
                }


                onSelectFunction?.Invoke(customFunc);
            }


        }

        // Method to find all types that inherit from a given class
        private IEnumerable<Type> FindInheritedClasses<T>()
        {
            return Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract).Where(x => x != typeof(CustomFunctionPrompt));
        }
        // Method to create a texture of a given color with rounded corners
        private Texture2D MakeTex(int width, int height, int cornerRadius, Color color)
        {
            Color[] pix = new Color[width * height];
            float radiusSquared = cornerRadius * cornerRadius;
            int midWidth = width / 2;
            int midHeight = height / 2;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = y * width + x;

                    float dx = Mathf.Abs(x - midWidth);
                    float dy = Mathf.Abs(y - midHeight);

                    if (dx * dx + dy * dy < radiusSquared)
                    {
                        pix[index] = color;
                    }
                    else
                    {
                        pix[index] = Color.clear; // Set transparent for non-rounded corners
                    }
                }
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

    }
}