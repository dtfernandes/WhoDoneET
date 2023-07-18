using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField] private CameraController _controller;
    [SerializeField] private Transform _inspectPosition;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pichUpLayerMask;

    [SerializeField] private Image _interactorIcon;

    [Header("Icons")]
    [SerializeField] private Sprite _dialogueIcon;

    private Interactable _focusItem;
    private PickupableObject _grabbedObject;

    void OnInteract()
    {            
        if (_focusItem != null)
        {
            if (_focusItem is PickupableObject)
            {
                if(_grabbedObject == null)
                _grabbedObject = _focusItem as PickupableObject;
                _grabbedObject.Grab(_inspectPosition);
                _controller.Inspect(_grabbedObject);
            }
            else
            {
                _grabbedObject.Drop();
                _controller.Inspect(null);
                _grabbedObject = null;
                _focusItem = null;
            }
        }    
    }

    public void Update()
    {
        float pickUpDistance = 10f;

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance)) {

            if (raycastHit.transform.TryGetComponent(out Interactable item))
            {
                _interactorIcon.enabled = true;
                _interactorIcon.sprite = _dialogueIcon;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Ray r = new Ray(transform.position, playerCameraTransform.forward);
        Gizmos.DrawRay(r);
    }
}
