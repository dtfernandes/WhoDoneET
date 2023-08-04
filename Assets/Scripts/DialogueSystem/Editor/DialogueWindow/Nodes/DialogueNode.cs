using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Class responsible for storing the data of a DialogueNode
    /// </summary>
    public class DialogueNode : DefaultNode
    {

        /// <summary>
        /// Name of the entity connected to this dialogue
        /// </summary>
        public int PresetName { get; private set; }
        public int ExpresionID { get; private set; }

        public TextField TextField { get; set; }
       
        public List<EventTriggerData> Events { get; set; }

        /// <summary>
        /// Text component of the Dialogue Node
        /// </summary>
        public string DialogText { get; set; }

        /// <summary>
        /// Property that defines if this node is connected to the Node "Start"
        /// </summary>
        public bool EntryPoint { get; set; }

        private DialogueNodeInspector inspector;


        private SavingWaitingList saveWatingList;
        public SavingWaitingList SaveWatingList { get => saveWatingList; set => saveWatingList = value; }
      

        public DialogueNode(SavingWaitingList savingWaitingList, NodeData nd = null)
        {
            //Setup node Default Style
            style.minWidth = 220;


            SaveWatingList = savingWaitingList;
            GUID = nd == null ? Guid.NewGuid().ToString() : nd.GUID;
            DialogText = nd == null ? "" : nd.Dialogue;
            Events = nd == null ? new List<EventTriggerData> { } : nd.Events;

            if (nd != null)
                foreach (ChoiceData cd in nd.OutPorts)
                    InstatiateChoicePort(cd.ChoiceText, cd.ID);

            //Create and add button that creates choice components
            Button button = new Button(clickEvent: () =>
            {
                InstatiateChoicePort();
            });
            button.text = "+";
            button.style.width = 40;
            titleContainer.Clear();
            titleContainer.Insert(0,button);

            Port port = InstatiateInputPort();
            inputContainer.Add(port);
            port.portName = "Input";
                                 
            //Create and Add a textField to input the dialogue
            TextField text = new TextField();
            TextField = text;

            text.RegisterCallback<ChangeEvent<string>>((ChangeEvent<string> evt) =>
            {
                DialogText = evt.newValue;
            });
            text.multiline = true;

            if (nd == null)
                text.value = $"\n\n\n";
            else
                text.value = $"{nd?.Dialogue}";

            mainContainer.Insert(1, text);
       
            #region Entity Preset PopUp
            
            //Load Entities and their presets 
            EntityData data = Resources.Load<EntityData>("EntityData");
            List<string> names = data.presetNames;

            //Get the selected preset
            int firstSelected = 0;
            int firstExpressionSelected = 0;
            bool expressionVisibility = false;
            if (nd != null)
            {
                firstSelected = nd.PresetName;
                firstExpressionSelected = nd.ExpressionId;          
            }

            if (firstSelected != 0)
            {
                expressionVisibility = true;
            }

            try
            {
                PopupField<string> justfortest = new PopupField<string>(names, firstSelected);
            }
            catch 
            {
                firstSelected = 0;
                Debug.LogWarning("Dude... The preset list disapeared somehow again");
            }

            PopupField<string> presetPopUp = new PopupField<string>(names, firstSelected);

            EntityInfo selectedInfo = data.data[firstSelected];
            List<string> expressionNames = new List<string> { };

            if (firstSelected != 0)
            {
                selectedInfo = data.data[firstSelected - 1];

                Debug.Log(selectedInfo);
                Debug.Log(selectedInfo.Expressions);

                expressionNames = selectedInfo.Expressions.Emotions.Select(x => x.EmotionName).ToList();              
            }

            PopupField<string> expressionPopUp = new PopupField<string>(expressionNames, firstExpressionSelected);


            PresetName = nd == null ? presetPopUp.index : nd.PresetName;
            ExpresionID = nd == null ? expressionPopUp.index : nd.ExpressionId;
           

            presetPopUp.RegisterCallback<ChangeEvent<string>>((ChangeEvent<string> evt) =>
            {
                //Choose a preset entity event

                int selectedPresetIndex = presetPopUp.index;

                //If its not default
                if (selectedPresetIndex != 0)
                {
                    extensionContainer.style.height = 47f;
                    EntityInfo selectedInfo = data.data[selectedPresetIndex - 1];

                    List<string> expressionNames = selectedInfo.Expressions.Emotions.Select(x => x.EmotionName).ToList();

                    expressionPopUp.choices = expressionNames;

                    int selectedClampedIndex = Math.Clamp(expressionPopUp.index, 0, expressionNames.Count - 1);

                    expressionPopUp.value = expressionNames[selectedClampedIndex];
                
                    ExpresionID = selectedClampedIndex;
                }
                else
                {
                    extensionContainer.style.height = 25;
                }

               
                PresetName = selectedPresetIndex;

                if (selectedPresetIndex != 0)
                {
                    expressionPopUp.visible = true;
                }
                else
                {
                    expressionPopUp.visible = false;
                }


                EnableInspectorDisplay();
            });

            expressionPopUp.RegisterCallback<ChangeEvent<string>>((ChangeEvent<string> evt) =>
            {
                ExpresionID = expressionPopUp.index;
                EnableInspectorDisplay();
            });


            extensionContainer.Add(presetPopUp);
            extensionContainer.Add(expressionPopUp);

            presetPopUp.style.marginTop = 2.5f;
            presetPopUp.style.marginBottom = 2.5f;
            extensionContainer.style.height = 25;
            expressionPopUp.visible = expressionVisibility;

          
            #endregion

            RegisterCallback<PointerDownEvent>((PointerDownEvent evt) =>
            {             
                EnableInspectorDisplay();
            });

            RefreshExpandedState();

        }

        private void EnableInspectorDisplay(){

            inspector =
                  ScriptableObject.CreateInstance("DialogueNodeInspector")
                       as DialogueNodeInspector;
            inspector.init(this);
            Selection.activeObject = inspector;

            TextField.UnregisterCallback<ChangeEvent<string>>(ChangeText);          
            TextField.RegisterCallback<ChangeEvent<string>>(ChangeText);
        }

        private void ChangeText(ChangeEvent<string> evt)
        {
            inspector.ChangeDialogue(evt.newValue, true);
        }

        private void InstatiateChoicePort(string choice = "", string id = "")
        {
            ChoiceData cD = new ChoiceData(choice, id);
            OutPorts.Add(cD);
            int index = OutPorts.IndexOf(cD);


            Port port = InstatiateOutputPort();
            port.portName = "";

            //Create and Add a textField to input the dialogue
            TextField textNode = new TextField();
        
            textNode.value = choice;
            textNode.multiline = true;

            textNode.RegisterCallback<ChangeEvent<string>>((ChangeEvent<string> evt) =>
            {
                if (port.connected)
                {
                    OutPorts[index].ChangeText(evt.newValue);
                }
            });


            //The Event responsible for managing the connetions of two ports
            port.RegisterCallback<MouseUpEvent>((MouseUpEvent evt) =>
            {             
                if (port.connected)
                {
                    foreach (Edge e in port.connections)
                    {
                        OutPorts[index].ChangeId((e.input.node as DefaultNode).GUID);
                        
                        e.RegisterCallback<DetachFromPanelEvent>
                        ((DetachFromPanelEvent evnt) =>
                        {
                            OutPorts[index].ChangeId("");
                        });
                    }
                }
            });

            //Create and Add Button to delete a Choice Port
            Button deleteButton = new Button();
       
            deleteButton.text = "X";
            
            VisualElement choiceContainer = new VisualElement();
            choiceContainer.Add(deleteButton);
            choiceContainer.Add(textNode);
            choiceContainer.Add(port);
            choiceContainer.styleSheets.Add(
                Resources.Load<StyleSheet>("ChoiceComponent_Style"));

            deleteButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
            {           
                foreach (Edge e in port.connections)
                {
                    e.input.Disconnect(e);

                    e.parent.Remove(e);
                }
                outputContainer.Remove(choiceContainer);
                OutPorts.Remove(cD);
                
            });


            outputContainer.Add(choiceContainer);

            RefreshExpandedState();
        }
    
        public void SetInInitialPosition(GraphView graph)
        {
            float width = graph.parent.contentRect.width;
            float height = graph.parent.contentRect.height;

            float x = -graph.viewTransform.position.x +
                width * 0.1f;

            float y = -graph.viewTransform.position.y +
                height * 0.1f;
                                 
            Vector2 position = new Vector2(x, y);
            Vector2 size = GetPosition().size;

            //Accout for node size
            //This is not implemented
            Vector2 pos = new Vector2(
                position.x - size.x / 2,
                position.y - size.y / 2
               );

            Rect rect = new Rect(position, size);
            SetPosition(rect);
        }

        public void SetInInitialPosition(Rect position)
        {
            SetPosition(position);
        }
   
        public void ConnectPorts(NodeData nd, DialogueGraphView graph)
        {
            //foreach(ChoiceComponent)
        }
    
    }
}
