using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionMarker : MonoBehaviour
{
    [field: SerializeField]
    public DialogueScript DescriptionScript { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, 0.01f);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.3f);
    }
}
