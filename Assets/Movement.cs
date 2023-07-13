using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


/// <summary>
/// Handles the movement of a controlable entity
/// </summary>
public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _movSpeed;

    //Rigidbody component
    private Rigidbody _rigid;
    //Value that manages the input information
    private Vector2 _inputValue;
    private Transform _cameraTransform;
    private PlayerInput _playerInput;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.ActivateInput();
    }

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void OnEnable() => _playerInput.enabled = true;
    private void OnDisable() => _playerInput.enabled = false;

    private void FixedUpdate()
    {
        if (GameSettings.Instance.isWorldStopped) return;

        Vector3 cameraForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = cameraForward * _inputValue.y + _cameraTransform.right * _inputValue.x;
        _rigid.velocity = movement * _movSpeed;
    }

    void OnMove(InputValue value)
    {
        _inputValue = value.Get<Vector2>();
    }

}
