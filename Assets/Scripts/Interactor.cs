using DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling the interaction action of the user
/// </summary>
public class Interactor : MonoBehaviour
{
    // Camera controller component of the player 
    [SerializeField] private CameraController _controller;

    //Position where an object will go when inspected
    [SerializeField] private Transform _inspectPosition;

    //Layer to only affect certain objects to interact 
    [SerializeField] private LayerMask _interactLayerMask;

    //Component that displays the interact icon
    [SerializeField] private Image _interactorIcon;

    #region Icons 
    [Header("Icons")]
    [SerializeField] private Sprite _dialogueIcon;
    [SerializeField] private Sprite _propIcon;
    [SerializeField] private Sprite _generalIcon;
    #endregion 

    //Item,  that can be interacted, overed by the player
    private Interactable _focusItem;

    //Item currently being grabbed
    private PickupableObject _grabbedObject;

    //Item currently being dialogued
    private DialogueInteractable _dialoguedObject;

    public System.Action<InteractionType> onInteract;

    // 
    private bool _inDescription;

    //Global Settings of the game
    private GameSettings _gameSettings;

    //Method that is triggered when the user uses the Interact key
    //Uses the new input system
    void OnInteract()
    {
        if (_gameSettings.isMenuOpen) return;

        if (_inDescription) return;

        //Check if the player is looking at an object
        if (_focusItem != null)
        {
            //Check if the object is an object that can be pick up
            if (_focusItem is PickupableObject)
            {
                if (_grabbedObject == null)
                {
                    //Stop time
                    _gameSettings.isWorldStopped = true;

                    // Convert object
                    _grabbedObject = _focusItem as PickupableObject;

                    //Setup object
                    _grabbedObject.Grab(_inspectPosition);

                    //Setup Camera
                    _controller.Inspect(_grabbedObject);

                    onInteract?.Invoke(InteractionType.Object);
                }
                else
                {
                    //Restart time
                    _gameSettings.isWorldStopped = false;

                    //Setup object
                    _grabbedObject.Drop();

                    //Setup Camera
                    _controller.Inspect(null);

                    _grabbedObject = null;
                    _focusItem = null;
                }
            }
            //Check if the object is an object that can be talked to
            else if (_focusItem is DialogueInteractable)
            {
                //Ignore while stopped
                if (_gameSettings.isWorldStopped) return;

                DialogueInteractable _dialogueObj = _focusItem as DialogueInteractable;

                _dialoguedObject = _dialogueObj;

                _controller.ZoomForDialogue(_dialogueObj);
                _gameSettings.isWorldStopped = true;

                _dialogueObj.OnEndDialogue += OnEndDialogue;
                _dialogueObj.StartDialogue();

                //Unlock mouse
                _gameSettings.LockCursor(false);
                _interactorIcon.gameObject.SetActive(false);

                onInteract?.Invoke(InteractionType.Dialogue);
            }
            else
            {
                //Ignore while stopped
                if (_gameSettings.isWorldStopped) return;

                
            }
        }    

        void OnEndDialogue(IDialogueScript script)
        {           
            _controller.ZoomOutDialogue();
            _gameSettings.LockCursor(true);
            _interactorIcon.gameObject.SetActive(true);

            //Remove the end dialogue function after is called
            _dialoguedObject.RemoveEvents();
        }
    }

    void OnInspect()
    {
        if (GameSettings.Instance.isMenuOpen) return;

        //Check if the player is looking at an object
        if (_grabbedObject != null)
        {
            //Check if the object is an object that can be pick up
            if (_focusItem is PickupableObject)
            {
                PickupableObject obj = _focusItem as PickupableObject;
                _controller.Stop();
              
                obj.StartDescription();
                
                DialogueDisplayHandler ddh = _gameSettings.DialogueHandler;
         
                _inDescription = true;
                _gameSettings.LockCursor(false,true);

                _grabbedObject = null;
                _focusItem = null;
            
                ddh.onEndDialogue += EndDescription;
                onInteract?.Invoke(InteractionType.Dialogue);
            }
        }

        void EndDescription(IDialogueScript script)
        {
            _gameSettings.LockCursor(true,true);
            _grabbedObject = _focusItem as PickupableObject;
            _inDescription = false;
            //Setup Camera
            _controller.Inspect(_grabbedObject);

            //Remove the end dialogue function after is called
            DialogueDisplayHandler ddh = _gameSettings.DialogueHandler;
            ddh.onEndDialogue -= EndDescription;
        }
    }

    public void Start()
    {
        _gameSettings =  GameSettings.Instance;
    }

    //Update
    public void Update()
    {
        float pickUpDistance = 10f;

        if (Physics.Raycast(_controller.transform.position, _controller.transform.forward, out RaycastHit raycastHit, pickUpDistance, _interactLayerMask)) {

            if (raycastHit.transform.TryGetComponent(out Interactable item))
            {
                _interactorIcon.enabled = true;
                if (item is PickupableObject)
                {
                    _interactorIcon.sprite = _propIcon;
                }
                else if (item is DialogueInteractable)
                {
                    if ((item as DialogueInteractable).PresetEntity < 0)
                    {
                        _interactorIcon.sprite = _propIcon;
                    }
                    else
                    {
                        _interactorIcon.sprite = _dialogueIcon;
                    }
                }
                else 
                {
                    _interactorIcon.sprite = _generalIcon;
                }
                _focusItem = item;
            }
            else
            {
                _interactorIcon.enabled = false;
                _focusItem = null;
            }
        }
        else
        {
            _interactorIcon.enabled = false;
            _focusItem = null;
        }
    }

    //Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Ray r = new Ray(transform.position, _controller.transform.forward);
        Gizmos.DrawRay(r);
    }
}

public enum InteractionType
{
    Dialogue,
    Object
}

