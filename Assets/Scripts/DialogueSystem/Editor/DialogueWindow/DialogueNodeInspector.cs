using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem.Editor
{
    public class DialogueNodeInspector : ScriptableObject
    {
        [SerializeField]
        private bool isStart;

        [SerializeField] [HideInInspector]
        private string dialogueText;

        private DialogueNode node;

        [HideInInspector]
        public List<EventTriggerData> events;

        [field: SerializeField][HideInInspector]
        public List<CustomFunction> CustomFunctions { get; set; }

        //[SerializeField]
        private EntityInfo entityInfo;
        
        private int presetEntityName;

        private SavingWaitingList saveWaitingList;
        public SavingWaitingList SaveWaitingList { get => saveWaitingList; set => saveWaitingList = value; }

        public void init(DialogueNode dn)
        {
            node = dn;
            isStart = dn.EntryPoint;
            dialogueText = dn.DialogText;
            events = node.Events;
            presetEntityName = dn.PresetName;
            SaveWaitingList = dn.SaveWatingList;

            CustomFunctions = new List<CustomFunction> { };
            CustomFunctions.AddRange(dn?.CustomFunctions);

            EntityData data = Resources.Load<EntityData>("EntityData");
            entityInfo = data.data[presetEntityName];
        }

        public void UpdateNode()
        {
            node.CustomFunctions = CustomFunctions;
        }

        public void ChangeDialogue(string newDialogue, bool fromTxt = false)
        {
            if (!fromTxt)
            {
                node.TextField.value = newDialogue;
                node.DialogText = newDialogue;
                //node.entityInfo = 
            }

            dialogueText = newDialogue;
        }
    }
}

