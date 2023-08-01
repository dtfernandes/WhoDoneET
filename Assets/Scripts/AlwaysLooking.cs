using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLooking : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }
    }
}
