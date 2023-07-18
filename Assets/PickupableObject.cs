using UnityEngine;
using UnityEngine.InputSystem;

public class PickupableObject : Interactable
{
    private Vector3 _originalPos;
    private Vector3 _originalRot;
    private Transform _inspectPosition;
    private bool _isGrabbed;
    private float _lerpTime = 10f;
    private Rigidbody _rigid;
    private bool _moving;


    private void Awake()
    {
        _originalPos = transform.position;
        _originalRot = transform.eulerAngles;
        _rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_moving) return;
        if (_isGrabbed && _inspectPosition != null)
        {
            // Interpolate the position to the inspect position
            transform.position = Vector3.Lerp(transform.position, _inspectPosition.position, _lerpTime * Time.deltaTime);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, _inspectPosition.eulerAngles, _lerpTime * 30f * Time.deltaTime);

            if(Vector3.Distance(transform.position, _inspectPosition.position) <= 0.05f)
            {
                _moving = false;
            }
        }
        else
        {
            // Interpolate the position back to the original position
            transform.position = Vector3.Lerp(transform.position, _originalPos, _lerpTime * Time.deltaTime);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, _originalRot, _lerpTime * 30 * Time.deltaTime);

            if (Vector3.Distance(transform.position, _originalPos) <= 0.05f)
            {
                _moving = false;
            }
        }
    }

    public void Drop()
    {
        _isGrabbed = false;
        _inspectPosition = null;
        _rigid.velocity = Vector3.zero;
        _moving = true;
    }

    public void Grab(Transform inspectPosition)
    {
        _isGrabbed = true;
        _inspectPosition = inspectPosition;
        _rigid.useGravity = false;
        _moving = true;
    }

}
