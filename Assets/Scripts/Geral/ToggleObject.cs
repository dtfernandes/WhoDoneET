using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleObject : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _event;

    public void InvokeEvent()
    {
        _event?.Invoke();
    }
}
