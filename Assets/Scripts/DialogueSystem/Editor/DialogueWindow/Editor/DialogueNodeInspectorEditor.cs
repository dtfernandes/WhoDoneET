﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    [CustomEditor(typeof(DialogueNodeInspector))]
    public class DialogueNodeInspectorEditor : UnityEditor.Editor
    {
        SerializedProperty dialogueText;
        SerializedProperty events;
        DialogueNodeInspector nodeInsp;

        int funcSelectPopup;

        private List<CustomFunction> _customFuctions;

        void OnEnable()
        {
            nodeInsp = target as DialogueNodeInspector;

            dialogueText = serializedObject.FindProperty("dialogueText");
            events = serializedObject.FindProperty("events");
            _customFuctions = nodeInsp.CustomFunctions;
            if(_customFuctions == null) _customFuctions= new List<CustomFunction>();
        }

        public override VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {           

            nodeInsp = target as DialogueNodeInspector;

            serializedObject.Update();

            base.OnInspectorGUI();

            string dialTxtTemp = dialogueText.stringValue; 
            EditorGUILayout.PropertyField(dialogueText, new GUIContent("Dialogue Text"));
            if (dialTxtTemp != dialogueText.stringValue)
                nodeInsp.ChangeDialogue(dialogueText.stringValue);

            GUILayout.BeginHorizontal();
          
            if(GUILayout.Button("Add Event"))
            {
                events.InsertArrayElementAtIndex(events.arraySize);
                SerializedProperty ev = events.GetArrayElementAtIndex(events.arraySize - 1);
                SerializedProperty gameObj =
                   ev.FindPropertyRelative("gameObj");
                SerializedProperty functionName =
                   ev.FindPropertyRelative("functionName");
                SerializedProperty index =
                   ev.FindPropertyRelative("indexPos");
                SerializedProperty showText =
                   ev.FindPropertyRelative("showText");

                gameObj.objectReferenceValue = null;
                functionName.stringValue = "";
                index.intValue = 0;
                showText.boolValue = false;


            }
           
            if (GUILayout.Button("Clear"))
            {
                for (int i = events.arraySize - 1; i >= 0; i--)
                {
                    RemoveEventFromManager(i);
                }


                events.ClearArray();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            EditorGUI.indentLevel = 2;
      
            //Draw Event Trigger Data
            for (int i = 0; i < events.arraySize; i++)
            {
                
                #region Assign Serialized Properties
                SerializedProperty gameObj =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("gameObj");
                SerializedProperty functionName =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("functionName");
                SerializedProperty index =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("indexPos");
                SerializedProperty showText =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("showText");
                SerializedProperty showSelf =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("showSelf");
                SerializedProperty uniqueID =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("uniqueID");
                SerializedProperty savedID =
                  events.GetArrayElementAtIndex(i).FindPropertyRelative("savedID");
                SerializedProperty listOfType =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("listOfType");
                SerializedProperty selectedComponent =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("selectedComponent");
                SerializedProperty seletedTypeIndex =
                   events.GetArrayElementAtIndex(i).FindPropertyRelative("seletedTypeIndex");
                SerializedProperty seletedMethodIndex =
                  events.GetArrayElementAtIndex(i).FindPropertyRelative("seletedMethodIndex");
                SerializedProperty listOfAssemblies =
                  events.GetArrayElementAtIndex(i).FindPropertyRelative("listOfAssemblies");
                #endregion

                GUILayout.Space(5);
                if (EditorGUILayout.DropdownButton(new GUIContent($"Event { i }"), FocusType.Passive))
                {
                    showSelf.boolValue = !showSelf.boolValue;
                }

                if (showSelf.boolValue)
                {
                    GUILayout.Space(10);

                    #region Show Text Toggle
                    EditorGUILayout.PropertyField(showText);
                    if (showText.boolValue)
                        DrawTriggerLabel(index.intValue, dialogueText.stringValue);
                    #endregion

                    #region Selected Letter Index Trigger
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(28);
                    GUILayout.Label("Index");
                    index.intValue = (int)EditorGUILayout.Slider(index.intValue, 0, dialogueText.stringValue.Length - 1);
                    GUILayout.EndHorizontal();
                    #endregion

                    #region GameObject Selection
                    GameObject gameObjTemp = gameObj.objectReferenceValue as GameObject;
                    EditorGUILayout.PropertyField(gameObj);

                    //On Change Game Object
                    if (gameObjTemp != gameObj.objectReferenceValue)
                    {

                        GameObject newGameObj = gameObj?.objectReferenceValue as GameObject;
                        GameObject oldObj = gameObjTemp;

                        //DialogueUniqueId idOld = gameObjTemp?.GetComponent<DialogueUniqueId>();
                        //DialogueUniqueId idNew =
                        //    (newGameObj)?.GetComponent<DialogueUniqueId>();

                        //Deselect Old Object
                        if (oldObj != null)
                        {
                            //Compare the objects in the wating list
                            if (uniqueID.stringValue == 
                                savedID.stringValue)
                            {
                                //Object Changed from One In Used
                                nodeInsp.SaveWaitingList.WaitListToDelete.Add(oldObj);
                            }
                            else
                            {
                                //Object Changed from one who is not In use
                                nodeInsp.SaveWaitingList.WaitListToAdd.Remove(oldObj);
                            }
                            //Remove The object equal to this one 
                            //nodeInsp.SaveWaitingList.WaitListToAdd.Remove(oldObj);
                        }

                        //New Object Selected
                        if (newGameObj != null)
                        {
                            #region Create List of Types and Assemblies
                            Component[] components = newGameObj.GetComponents(typeof(Component));
                            List<System.Type> listType = new List<Type> { };
                            foreach (Component component in components)
                            {
                                listType.Add(component.GetType());
                            }

                            listOfType.ClearArray();

                            for (int o = 0; o < listType.Count; o++)
                            {
                                listOfType.InsertArrayElementAtIndex(listOfType.arraySize);
                                listOfAssemblies.InsertArrayElementAtIndex(listOfAssemblies.arraySize);

                                listOfType.GetArrayElementAtIndex(o).stringValue = listType[o].ToString();
                                listOfAssemblies.GetArrayElementAtIndex(o).stringValue =
                                    Assembly.GetAssembly(listType[o]).ToString();
                            }
                            #endregion 

                            string id = DialogueEventManager.GetID(newGameObj);
                            uniqueID.stringValue = id;
                            savedID.stringValue = id;

                            if (uniqueID.stringValue ==
                                savedID.stringValue)
                            {

                            }


                            nodeInsp.SaveWaitingList.WaitListToAdd.Add(newGameObj);
                            
                            
                            //if (idNew)
                            //{
                            //    uniqueID.stringValue = idNew.UniqueID;
                            //}
                            //else
                            //{
                            //    DialogueUniqueId newId = newGameObj.AddComponent<DialogueUniqueId>();
                            //    newId.UniqueID = Guid.NewGuid().ToString();
                            //    uniqueID.stringValue = newId.UniqueID;
                            //}
                        }
                        else
                        {
                            uniqueID.stringValue = "";
                        }
                    }
                    #endregion

                    #region Component Type PopUp
                    if (listOfType.arraySize > 0)
                    {
                        string[] typeNameList = new string[listOfType.arraySize];
                        for (int o = 0; o < listOfType.arraySize; o++)
                        {
                            typeNameList[o] = listOfType.GetArrayElementAtIndex(o).stringValue;
                        }

                        seletedTypeIndex.intValue =
                        EditorGUILayout.Popup(new GUIContent("Object Components"), seletedTypeIndex.intValue, typeNameList);
                    }
                    #endregion

                    #region Method PopUp
                    if (listOfType.arraySize > 0)
                    {
                        string assemblyName = listOfAssemblies.GetArrayElementAtIndex(seletedTypeIndex.intValue).stringValue;
                        string type = listOfType.GetArrayElementAtIndex(seletedTypeIndex.intValue).stringValue;
                        string qualifiedName = Assembly.CreateQualifiedName(assemblyName, type);
                        System.Type typeSelected = Type.GetType(qualifiedName);

                        MethodInfo[] methodInfo = typeSelected.GetMethods();
                        string[] methodNames = methodInfo.Select(x => x.Name).ToArray();
                        seletedMethodIndex.intValue =
                            EditorGUILayout.Popup(new GUIContent("Methods"), seletedMethodIndex.intValue, methodNames);
                        functionName.stringValue = methodNames[seletedMethodIndex.intValue];
                    }
                    #endregion

                    #region Unique Id
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    string idField = uniqueID.stringValue != "" ? uniqueID.stringValue : "no id";
                    GUILayout.Label(idField);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    #endregion

                    EditorGUILayout.PropertyField(functionName);


                    GUILayout.Space(10);

                    if (GUILayout.Button("Remove Event"))
                    {
                        //Remove Event From Manager
                        RemoveEventFromManager(i);
                        events.DeleteArrayElementAtIndex(i);
                    }

                }

                GUILayout.Space(10);
            }

            EditorGUI.indentLevel = 1;


            GUILayout.Space(10);

            if (GUILayout.Button("Add Custom Event"))
            {
                CustomFunctionPrompt prompt = new CustomFunctionPrompt();
                SetupCustomFunction(prompt);
            }

            GUILayout.Space(10);

            for (int i = 0; i < _customFuctions.Count; i++)
            {
                 CustomFunction func = _customFuctions[i];
                 func.Draw();
            }
          

            serializedObject.ApplyModifiedProperties();
        }

        private void SetupCustomFunction(CustomFunctionPrompt prompt)
        {
            int index = _customFuctions.Count;
            _customFuctions.Add(prompt);
            prompt.OnEnable();

            nodeInsp.UpdateNode();

            prompt.onSelectFunction += (c) =>
            {
                c.GUID = GUID.Generate().ToString();
                
                _customFuctions[index] = c;

                c.OnEnable();

                nodeInsp.UpdateNode();

                c.onUpdate += () =>
                {
                    nodeInsp.UpdateNode();
                };
            };
        }

        private void DrawTriggerLabel(int intValue, string text)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            
            for (int i = 0; i < text.Length; i++)
            {
                if (i % 25 == 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
   
                if (intValue == i)
                {
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.red;
                    style.fontSize = 20;
                    GUILayout.Label(text[i].ToString(), style);
                }
                else
                {
                    GUILayout.Label(text[i].ToString());
                }

             
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
     
        
        private void RemoveEventFromManager(int index)
        {
            SerializedProperty gameObj =
                   events.GetArrayElementAtIndex(index).FindPropertyRelative("gameObj");
            SerializedProperty uniqueID =
                  events.GetArrayElementAtIndex(index).FindPropertyRelative("uniqueID");
            SerializedProperty savedID =
              events.GetArrayElementAtIndex(index).FindPropertyRelative("savedID");

            GameObject oldObj = gameObj.objectReferenceValue as GameObject;

            //Deselect Old Object
            if (oldObj != null)
            {
                //Compare the objects in the wating list
                if (uniqueID.stringValue ==
                    savedID.stringValue)
                {
                    //Object Changed from One In Used
                    nodeInsp.SaveWaitingList.WaitListToDelete.Add(oldObj);
                }
                else
                {
                    //Object Changed from one who is not In use
                    nodeInsp.SaveWaitingList.WaitListToAdd.Remove(oldObj);
                }
                //Remove The object equal to this one 
                //nodeInsp.SaveWaitingList.WaitListToAdd.Remove(oldObj);
            }
        }

    }
}
