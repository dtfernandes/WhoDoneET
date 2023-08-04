﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


namespace DialogueSystem.Editor
{
    /// <summary>
    /// Class responsible for managing the graphview component of
    /// the Node based Dialogue System
    /// </summary>
    public class DialogueGraphView : GraphView
    {
        /// <summary>
        /// Property that defines the name of the dislayed Dialogue
        /// </summary>
        public string DialogueName { get; set; }
        
        private DropdownMenuAction.Status hideStatus;

      
        private SavingWaitingList saveWatingList;
        public SavingWaitingList SaveWatingList { get => saveWatingList; set => saveWatingList = value; }
              
        /// <summary>
        /// Constructor of this class
        /// </summary>
        public DialogueGraphView()
        {
            SaveWatingList = new SavingWaitingList();

            hideStatus = DropdownMenuAction.Status.Normal;
            SetupZoom(ContentZoomer.DefaultMinScale, 
               ContentZoomer.DefaultMaxScale);

            ContentDragger newDragger = new ContentDragger();
            
            this.AddManipulator(newDragger);
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
           
            AddElement(GenerateFirstNode());
        }

        

        /// <summary>
        /// Override that adds more menu options to the contextual menu 
        /// of the items
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu
            (ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            if (evt.target is Node)
            {
                Node node = evt.target as Node;
              

                evt.menu.InsertAction(0,"Text Only", (e) => 
                {

                    if (hideStatus != (DropdownMenuAction.Status)4)
                    {
                        (node as DialogueNode).SwitchVisibility(false);
                        hideStatus = DropdownMenuAction.Status.Checked;
                    }
                    else
                    {
                        hideStatus = DropdownMenuAction.Status.Normal;
                        (node as DialogueNode).SwitchVisibility(true);
                    }

                }, hideStatus);
            }
        }


        /// <summary>
        /// Method responsible for getting all ports 
        /// compatible with given port.
        /// </summary>
        /// <param name="startPort">Start port to validate against</param>
        /// <param name="nodeAdapter">NodeAdapter Component</param>
        /// <returns>List of compatible ports</returns>
        public override List<Port> GetCompatiblePorts(Port startPort,
            NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(funcCall: (port) =>
            {
                if (startPort != port && startPort.node != port.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

 
        /// <summary>
        /// Method responsible for creating the "Start" node of the 
        /// Dialogue System
        /// </summary>
        /// <returns>The first Node</returns>
        private StartNode GenerateFirstNode()
        {
            StartNode node = new StartNode();
            
            node.SetPosition(new Rect(100, 200, 100, 150));
            return node;
        }

        /// <summary>
        /// Method responsible for creating a new Dialogue Node
        /// </summary>
        /// <param name="nd">Data of the node to be created</param>
        public void CreateDialogueNode(NodeData nd = null)
        {
            DialogueNode node = new DialogueNode(SaveWatingList, nd);
            AddElement(node);
            if(nd == null)
                node.SetInInitialPosition(this); 
            else
                node.SetInInitialPosition(nd.Position);

        }

        //LOAD DIALOGUE SCRIPT

        /// <summary>
        /// Method responsible for adding the ports connections to the window
        /// </summary>
        /// <param name="data">Data to base the connections out of</param>
        public void InstatiateEdges(NodeData data)
        {
            //ConnectStart

            DialogueNode node = GetNode(data.GUID);
      
            int it = 0;

            foreach (UnityEngine.UIElements.VisualElement welp in node.outputContainer.Children())
            {
                foreach (UnityEngine.UIElements.VisualElement welp2 in welp.Children())
                {
                    if (!(welp2 is Port))
                    {
                        continue;
                    }

                    Port p = welp2 as Port;

                    if (data.OutPorts.Count == 0) continue;

                    string gui = data.OutPorts[it].ID;
                    DialogueNode conNode = GetNode(gui);

                    foreach (Port ort in conNode.inputContainer.Children())
                    {
                        AddElement(ort.ConnectTo(p));
                    }
                }

                it++;
            }

        }

        /// <summary>
        /// Method responsible for connecting the first Node 
        /// to the "Start" Node
        /// </summary>
        /// <param name="data">Data to base the connection out of</param>
        public void ConnectToStart(NodeData data)
        {           
            StartNode start = nodes.First() as StartNode;

            foreach (Port p in start.outputContainer.Children())
            {
                DialogueNode node = GetNode(data.GUID);

                node.EntryPoint = true;

                foreach (Port ort in node.inputContainer.Children())
                {
                    AddElement(ort.ConnectTo(p));
                }
            }
        }

        /// <summary>
        /// Method responsible for getting a Node based on its ID
        /// </summary>
        /// <param name="id">ID of the wanted Node</param>
        /// <returns>The Node with the passed ID</returns>
        public DialogueNode GetNode(string id)
        {
            DialogueNode z = null;
            nodes.ForEach(x =>
            {
                if ((x is DialogueNode))
                {
                    DialogueNode y = (x as DialogueNode);
                    if (y.GUID == id)
                        z = y;
                }
            });

            return z;

        }
    }


}