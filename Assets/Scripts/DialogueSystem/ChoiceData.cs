using System;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{

    /// <summary>
    /// Class responsible for storing the choice text and
    /// its respective id
    /// </summary>
    [Serializable]
    public class ChoiceData
    {

        /// <summary>
        /// Text of a Choice Component
        /// </summary>
        [UnityEngine.SerializeField]
        private string choiceText;
        
        /// <summary>
        /// Unique Id that represents the specific Choice
        /// </summary>
        [UnityEngine.SerializeField]
        private string id;

        /// <summary>
        /// Property that defines the choice text
        /// </summary>
        public string ChoiceText => choiceText;

        /// <summary>
        /// Property that defines the unique id of the specific Choice
        /// </summary>
        public string ID => id;
         
        public string ChoicePortID { get; set; }

        [field: UnityEngine.SerializeField]
        public bool IsLocked { get; set; }
        
        [field: UnityEngine.SerializeField]
        public bool IsHidden { get; set; }

        public Port Port { get; private set; }

        /// <summary>
        /// Constructor of this struct
        /// </summary>
        /// <param name="name">Choice text</param>
        /// <param name="id">Unique id of the Choice</param>
        public ChoiceData(string text, string id, Port port)
        {
            this.choiceText = text;
            this.id = id;
            Port = port;
        }

        public void ChangeText(string text)
        {
            choiceText = text;
        }

        public void ChangeId(string id)
        {
            this.id = id;
        }

        public ChoiceData Clone()
        {
            ChoiceData clone = new ChoiceData(ChoiceText, ID, Port);
            clone.IsHidden = IsHidden;
            clone.IsLocked = IsLocked;
            return clone;
        }
    }
}
