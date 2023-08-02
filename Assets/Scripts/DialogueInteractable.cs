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

    public Action OnEndDialogue { get; set; }

    [SerializeField]
    private DialogueController _controller;

    private DialogueDisplayHandler _dHandler;
    private SpriteRenderer _rederer;


    private void Awake()
    {
        _rederer = GetComponent<SpriteRenderer>();
      
    }

    public void StartDialogue()
    {
        _dHandler = GameSettings.Instance.DialogueHandler;
        if (!_dHandler.InDialogue)
        {
            _dHandler.StartDialolgue(_controller.GetDialogue());
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