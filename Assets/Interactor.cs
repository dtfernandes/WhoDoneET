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

    //Global Settings of the game
    private GameSettings _gameSettings;

    //Method that is triggered when the user uses the Interact key
    //Uses the new input system
    void OnInteract()
    {
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
              
                DialogueInteractable _dialogueObj = _focusItem as DialogueInteractable;
                if(!_gameSettings.isWorldStopped)
                     _controller.ZoomForDialogue(_dialogueObj);
                _gameSettings.isWorldStopped = true;
                _dialogueObj.onEndDialogue += _controller.ZoomOutDialogue;
                _dialogueObj.StartDialogue();                
            }
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
                    _interactorIcon.sprite = _dialogueIcon;
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

