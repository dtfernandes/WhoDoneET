using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 2f;
    private float _xRotation = 0f;
    private Transform _playerTransform;
    private InputAction _mouseAction;

    private PickupableObject _inspectingObject;
    [SerializeField] private float _rotationSpeed;
    Vector3 _mouseStartPosition;
    private bool _startInspect;

    // Start is called before the first frame update
    void Awake()
    {
        _playerTransform = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Inspect(PickupableObject grab)
    {
        _inspectingObject = grab;
        if(_inspectingObject)
            _startInspect = true;
    }

    private void OnLook(InputValue value)
    {
        if (_startInspect)
        {
            _mouseStartPosition = value.Get<Vector2>(); 
        }

        if (_inspectingObject)
        {
            Vector3 currentMousePosition = value.Get<Vector2>();
            Vector3 mouseInspectDelta = currentMousePosition - _mouseStartPosition;

            // Rotate the object based on mouse movement
            _inspectingObject.transform.Rotate(Vector3.up, -mouseInspectDelta.x * _rotationSpeed * Time.deltaTime, Space.World);

            _mouseStartPosition = currentMousePosition;
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
}
