using UnityEngine;

/// <summary>
/// Makethe object rotate to always look at the target
/// </summary>
public class AlwaysLooking : MonoBehaviour
{

    //Target for the object to always look at
    [SerializeField]
    private Transform _target;
    
    //Toggles that lock a certain axis of the rotation
    [Header("Axis Locks")]
    [SerializeField] private bool _lockXAxis;
    [SerializeField] private bool _lockYAxis;
    [SerializeField] private bool _lockZAxis;

    private void Update()
    {
        if (_target != null)
        {
            Vector3 direction = _target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            
            // Apply axis locks to the rotation
            Vector3 eulerRotation = rotation.eulerAngles;
            if (_lockXAxis) eulerRotation.x = 0f;
            if (_lockYAxis) eulerRotation.y = 0f;
            if (_lockZAxis) eulerRotation.z = 0f;

            transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }
}
