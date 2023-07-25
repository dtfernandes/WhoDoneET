using DialogueSystem;
using System;
using UnityEngine;

public class DialogueInteractable: Interactable
{
    public Action onEndDialogue;
    [SerializeField]
    private DialogueScript _script;

    public void StartDialogue()
    {
        DialogueDisplayHandler _dHandler =  GameSettings.Instance.DialogueHandler;

        if (!_dHandler.InDialogue)
        {
            _dHandler.StartDialolgue(_script);
            _dHandler.onEndDialogue -= onEndDialogue;
            _dHandler.onEndDialogue += onEndDialogue;
        }
    }
}