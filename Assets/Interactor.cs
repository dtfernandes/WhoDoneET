using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _inspectPosition;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pichUpLayerMask;

    private PickupableObject _grabbedObject;

    void OnInteract()
    {
        float pickUpDistance = 2f;
        if (_grabbedObject == null)
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance))
                if (raycastHit.transform.TryGetComponent(out PickupableObject grab))
                {
                    _grabbedObject = grab;
                    _grabbedObject.Grab(_inspectPosition);
                }
        }
        else
        {
            _grabbedObject.Drop();
            _grabbedObject = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Ray r = new Ray(transform.position, playerCameraTransform.forward);
        Gizmos.DrawRay(r);
    }
}
