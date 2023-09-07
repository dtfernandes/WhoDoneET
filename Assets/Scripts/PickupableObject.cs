using DialogueSystem;
using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PickupableObject : Interactable
{
   
    [SerializeField]
    private EventReference _pickUpSound;
    private EventInstance _pickSoundInstance;

    private Vector3 _originalPos;
    private Vector3 _originalRot;
    private Transform _inspectPosition;
    private bool _isGrabbed;
    private float _lerpTime = 10f;
    private Rigidbody _rigid;
    private bool _moving;

    public Action onGrab, onDrop;


    [field: Header("Dialogue")][field: SerializeField]
    public DialogueScript DefaultDescription { get; private set; }

    [SerializeField]
    private DescriptionMarker[] _descriptionMarker;


    private void Awake()
    {
        _originalPos = transform.position;
        _originalRot = transform.eulerAngles;
        _rigid = GetComponent<Rigidbody>();
        _pickSoundInstance = RuntimeManager.CreateInstance(_pickUpSound);
    }

    private void Update()
    {
        if (!_moving) return;

        if (_isGrabbed && _inspectPosition != null)
        {
            // Interpolate the position to the inspect position
            transform.position = Vector3.Lerp(transform.position, _inspectPosition.position, _lerpTime * Time.deltaTime);

            if(Vector3.Distance(transform.position, _inspectPosition.position) <= 0.05f)
            {
                _moving = false;
            }
        }
        else
        {
            // Interpolate the position back to the original position
            transform.position = Vector3.Lerp(transform.position, _originalPos, _lerpTime * Time.deltaTime);
            transform.eulerAngles =  _originalRot;

            if (Vector3.Distance(transform.position, _originalPos) <= 0.05f)
            {
                _moving = false;
            }
        }
    }

    public void Drop()
    {
        onDrop?.Invoke();
        _isGrabbed = false;
        _inspectPosition = null;
        _rigid.velocity = Vector3.zero;
        _moving = true;
    }

    public void Grab(Transform inspectPosition)
    {
        RuntimeManager.PlayOneShot(_pickUpSound);
        onGrab?.Invoke();
        _isGrabbed = true;
        _inspectPosition = inspectPosition;
        _rigid.useGravity = false;
        _moving = true;
    }

    public DialogueScript GetDescription()
    {
        foreach(DescriptionMarker marker in _descriptionMarker)
        {
            // Get the camera's transform
            Transform cameraTransform = Camera.main.transform; // Assuming you are using the main camera, change accordingly if not

            // Calculate the normalized forward vectors of the camera and the marker
            Vector3 cameraForward = cameraTransform.forward.normalized;
            Vector3 markerForward = marker.transform.forward.normalized;

            // Calculate the dot product between the camera and marker forward vectors
            float dotProduct = Vector3.Dot(cameraForward, markerForward);

            // Now, you can analyze the dot product to determine the orientation of the marker
            if (dotProduct < -0.7f)
            {
                return marker.DescriptionScript;
            }
        }

        return DefaultDescription;
    }
}

