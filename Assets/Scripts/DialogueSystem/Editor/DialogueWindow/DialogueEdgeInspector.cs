using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace DialogueSystem.Editor
{
    public class DialogueEdgeInspector : ScriptableObject
    {
        [SerializeField]
        private ChoiceData _choiceData;

        [SerializeField]
        private Edge _edge;
        

        public void init(Edge edge, ChoiceData choiceData)
        {
            _choiceData = choiceData;
            _edge = edge;
        }
    }
}