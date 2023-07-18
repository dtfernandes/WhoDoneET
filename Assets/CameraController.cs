using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float lerpSpeed = 5f;

    private DialogueInteractable dialogueObj;
    private float desiredDistance;

    [SerializeField] private float _sensitivity = 2f;
    private float _xRotation = 0f;
    private Transform _playerTransform;
    private InputAction _mouseAction;

    private PickupableObject _inspectingObject;
    [SerializeField] private float _rotationSpeed;
    Vector2 _mouseStartPosition;
    private bool _startInspect;
    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        _playerTransform = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
        _playerInput = GetComponent<PlayerInput>();
    }

    public void Inspect(PickupableObject grab)
    {
        _inspectingObject = grab;
        if(_inspectingObject)
            _startInspect = true;
    }

    private void OnLook(InputValue value)
    {
        if (_inspectingObject)
        {
            Vector2 currentMousePosition = value.Get<Vector2>();

            float rotationX = currentMousePosition.y * _rotationSpeed ;
            float rotationY = currentMousePosition.x * _rotationSpeed ;

            _inspectingObject.transform.Rotate(_inspectingObject.transform.up, rotationY, Space.World);
            _inspectingObject.transform.Rotate(_inspectingObject.transform.right, rotationX, Space.World);
          
        }

        if (GameSettings.Instance.isWorldStopped) return;

        Vector2 mouseDelta = value.Get<Vector2>();

        float mouseX = mouseDelta.x * _sensitivity;
        float mouseY = mouseDelta.y * _sensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerTransform.Rotate(Vector3.up * mouseX);
    }


    public void ZoomForDialogue(DialogueInteractable dialogueObj)
    {
        this.dialogueObj = dialogueObj;

        // Get the bounds of the dialogue object
        Renderer dialogueRenderer = dialogueObj.GetComponent<Renderer>();
        Bounds dialogueBounds = dialogueRenderer.bounds;

        // Calculate the size of the dialogue object
        float dialogueSize = Mathf.Max(dialogueBounds.size.x, dialogueBounds.size.y, dialogueBounds.size.z);

        // Calculate the desired distance based on the dialogue object size and camera field of view
        desiredDistance = dialogueSize / (2f * Mathf.Tan(Mathf.Deg2Rad * Camera.main.fieldOfView / 2f));
    }

    private void Update()
    {
        if (dialogueObj != null)
        {
            // Set the camera's position to be in front of the dialogue object along its normal
            Vector3 targetPosition = dialogueObj.transform.position + (dialogueObj.transform.forward * desiredDistance);
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);

            // Rotate the camera to face the dialogue object while following its normal
            Quaternion targetRotation = Quaternion.LookRotation(dialogueObj.transform.position - transform.position, dialogueObj.transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
        }
    }
}
