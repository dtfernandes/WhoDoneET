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

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(_inputValue.x, 0f, _inputValue.y);
        _rigid.velocity = movement * _movSpeed;
    }

    void OnMove(InputValue value)
    {
        _inputValue = value.Get<Vector2>();
    }

}
