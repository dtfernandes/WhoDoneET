using DialogueSystem;
using System;
using UnityEngine;

/// <summary>
/// Class that represents an entity that displays a dialogue
/// </summary>
public class DialogueInteractable: Interactable
{
    [field: SerializeField]
    public int PresetEntity { get; private set; }

    [field: SerializeField]
    public ZoomConfig Config { get; private set; }

    public Action<IDialogueScript> OnEndDialogue { get; set; }

    private DialogueDisplayHandler _dHandler;
    private SpriteRenderer _rederer;

    private DialogueController _controller;

    private void Awake()
    {
        _rederer = GetComponent<SpriteRenderer>();
        _controller = GetComponent<DialogueController>();
    }

    public void StartDialogue()
    {
        _dHandler = GameSettings.Instance.DialogueHandler;
        if (!_dHandler.InDialogue)
        {
            _controller.Play();
            _dHandler.onEndDialogue -= OnEndDialogue;
            _dHandler.onEndDialogue += OnEndDialogue;
        }
    }

    public void ChangeExpression(Sprite expression)
    {
        _rederer.sprite = expression;
    }

    public void RemoveEvents()
    {
        _dHandler.onEndDialogue -= OnEndDialogue;
        OnEndDialogue = null;
    }

    [Serializable]
    public struct ZoomConfig
    {
        [field:SerializeField]
        public float Top { get; private set; }
        
        [field: SerializeField]
        public float Down { get; private set; }

        [field: SerializeField]
        public Vector3 Center { get; private set; }
    }

}