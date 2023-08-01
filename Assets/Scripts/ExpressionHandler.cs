using DialogueSystem;
using System.Linq;
using UnityEngine;

public class ExpressionHandler : MonoBehaviour
{
    private DialogueDisplayHandler _dialogueDisplayHandler;

    [SerializeField]
    private DialogueInteractable[] _dialogueInteractables;

    private EntityData _presetData;

    private void Awake()
    {
        _dialogueDisplayHandler = GetComponent<DialogueDisplayHandler>();    
    }
    private void Start()
    {
        _dialogueDisplayHandler.onStartLine += ChangeExpression;
        _presetData = Resources.Load<EntityData>("EntityData");
    }

    private void ChangeExpression(NodeData nd)
    {
        //Get the current entity
        int id = nd.PresetName;

        if (id == 0)
            return;

        DialogueInteractable currentEntity = _dialogueInteractables.FirstOrDefault(x => x.PresetEntity == id-1);

        EntityInfo info = _presetData.data[id-1];

        currentEntity.ChangeExpression(info.Expressions.Emotions[nd.ExpressionId].Image);
    }

}

