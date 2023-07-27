using DialogueSystem;
using System;
using UnityEngine;


public class DialogueInteractable: Interactable
{
    public Action onEndDialogue;
    [SerializeField]
    private DialogueScript _script;
   
    [field: SerializeField]
    public int PresetEntity { get; private set; }
   
    private SpriteRenderer _image;

    private void Awake()
    {
        _image = GetComponent<SpriteRenderer>();
    }

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

    public void ChangeExpression(Sprite expression)
    {
        _image.sprite = expression;
    }
}