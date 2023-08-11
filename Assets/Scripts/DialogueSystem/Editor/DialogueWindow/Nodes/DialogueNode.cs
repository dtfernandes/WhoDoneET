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

        public List<CustomFunction> CustomFunctions { get; set; }

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

            Color _nodeAccentColor = new Color(0.17f, 0.17f, 0.17f);
            Color _nodeColor = new Color(0.2f,0.2f,0.2f);
        
            //Setup node Default Style
            style.minWidth = 220;

            CustomFunctions = new List<CustomFunction> { };

            if(nd?.CustomFunctions != null)
                CustomFunctions.AddRange(nd.CustomFunctions);

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

            button.style.width = 30;
            titleContainer.Clear();
            titleContainer.Insert(0,button);
            titleContainer.style.height = 30;
            titleContainer.style.backgroundColor = _nodeColor;

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

            VisualElement textContainer = new VisualElement();
            textContainer.style.paddingTop = 2;
            textContainer.style.paddingBottom = 2;
            textContainer.style.backgroundColor = _nodeAccentColor;


            if (nd == null)
                text.value = $"\n\n\n";
            else
                text.value = $"{nd?.Dialogue}";

            textContainer.Add(text);
            mainContainer.Insert(1,textContainer);
            //text.style.backgroundColor = _nodeColor;

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
            bool hasExpression = (selectedInfo?.Expressions?.Emotions?.Count ?? 0) > 0;
           
            if (firstSelected != 0 && hasExpression)
            {
                selectedInfo = data.data[firstSelected - 1];
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
                    EntityInfo selectedInfo = data.data[selectedPresetIndex - 1];

                    if ((selectedInfo?.Expressions?.Emotions?.Count ?? 0) > 0)
                    {

                        extensionContainer.style.height = 47f;

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
            extensionContainer.style.backgroundColor = _nodeColor;
            extensionContainer.style.height = 25;
            expressionPopUp.visible = false;

            if (presetPopUp.index != 0 && hasExpression)
            {
                expressionPopUp.visible = true;
                extensionContainer.style.height = 47f;
            }
 
          
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
            Color _xColor = new Color(0.4f,0.2f,0.2f);


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

            deleteButton.style.backgroundColor = _xColor;

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
