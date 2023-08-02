using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Scriptables/DialogueController")]
    public class DialogueController : ScriptableObject
    {
        [SerializeField]
        private List<DialogueScript> _dialogues;

        public DialogueScript GetDialogue()
        {
            return _dialogues[0];
        }
    }

}
