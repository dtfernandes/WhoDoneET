using UnityEngine;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 2f;
    private float _xRotation = 0f;
    private Transform _playerTransform;
    private InputAction _mouseAction;

    // Start is called before the first frame update
    void Awake()
    {
        _playerTransform = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    } 
    private void OnLook(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();

        float mouseX = mouseDelta.x * _sensitivity;
        float mouseY = mouseDelta.y * _sensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerTransform.Rotate(Vector3.up * mouseX);
    }
}
