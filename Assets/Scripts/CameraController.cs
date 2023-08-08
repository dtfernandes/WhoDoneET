 using System;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Control the camera movement during the game
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _lerpSpeed = 5f;
    [SerializeField] 
    private float _sensitivity = 2f;
    [SerializeField] 
    private float _rotationSpeed;
    
    private float _xRotation = 0f;

    private bool _zoomOut;


    private Vector3 _desiredPosition;
    private Vector3 _originalPosition;
    private Vector3 _originalRotation;

    private DialogueInteractable _dialogueObj;

    private PickupableObject _inspectingObject;

    private Transform _player;
    GameSettings _gameSettings;


    void Awake()
    {
        _gameSettings = GameSettings.Instance;
        _gameSettings.LockCursor(true);
        _player = transform.parent;
    }
  
    private void Update()
    {
        if (_dialogueObj != null)
        {
            transform.position = Vector3.Lerp(transform.position, _desiredPosition, _lerpSpeed * 0.5f * Time.deltaTime);

            // Rotate the camera to face the dialogue object while following its normal

            Vector3 position = _dialogueObj.transform.position + _dialogueObj.Config.Center;

            Quaternion targetRotation = Quaternion.LookRotation(position - transform.position, _dialogueObj.transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _lerpSpeed * Time.deltaTime);
        }

        if (_zoomOut)
        {
            transform.position = Vector3.Lerp(transform.position, _originalPosition, _lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_originalRotation), _lerpSpeed * 1.5f * Time.deltaTime);

            if (Vector3.Distance(_originalPosition, transform.position) < 0.1f)
            {
                _zoomOut = false;
                GameSettings.Instance.isWorldStopped = false;
            }
        }
    }

    private void OnLook(InputValue value)
    {
       
        if (_inspectingObject)
        {

            Vector2 currentMousePosition = value.Get<Vector2>();

            float rotationX = currentMousePosition.x * _rotationSpeed;
            float rotationY = currentMousePosition.y * _rotationSpeed;

            // Rotate around the up (vertical) axis for horizontal movement
            _inspectingObject.transform.Rotate(Vector3.up, rotationX, Space.World);

            // Rotate around the right (horizontal) axis for vertical movement
            _inspectingObject.transform.Rotate(Vector3.right, rotationY, Space.World);
        }

        if (_gameSettings.isWorldStopped || _gameSettings.isMenuOpen) return;


        Vector2 mouseDelta = value.Get<Vector2>();

        float mouseX = mouseDelta.x * _sensitivity;
        float mouseY = mouseDelta.y * _sensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _player.transform.Rotate(Vector3.up * mouseX);
    }

    public void Inspect(PickupableObject grab)
    {
        _inspectingObject = grab;
    }

    public void ZoomForDialogue(DialogueInteractable dialogueObj)
    {
        _originalRotation = transform.eulerAngles;
        _originalPosition = transform.position;
        this._dialogueObj = dialogueObj;

        float top = dialogueObj.Config.Top;
        float down = dialogueObj.Config.Down;

        // Get the bounds of the dialogue object
        Renderer dialogueRenderer = dialogueObj.GetComponent<Renderer>();
        Bounds dialogueBounds = dialogueRenderer.bounds;


        // Calculate the size of the dialogue object
        float dialogueSize = Mathf.Max(dialogueBounds.size.x, dialogueBounds.size.y, dialogueBounds.size.z);

        if (top != 0 || down != 0)
        {
            dialogueSize = top - down;
        }

        // Calculate the desired distance based on the dialogue object size and camera field of view
        float desiredDistance = dialogueSize / (2f * Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2f));
        // Set the camera's position to be in front of the dialogue object along its normal
        _desiredPosition = (dialogueObj.transform.position + dialogueObj.Config.Center) + (dialogueObj.transform.forward * desiredDistance);
    }

    public void ZoomOutDialogue()
    {
        _zoomOut = true;
        _dialogueObj = null;
    }

    public void Stop()
    {
        _inspectingObject = null;
    }
}
