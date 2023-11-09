using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _objectToActivate;

    public void ActivateObjectAt(int index)
    {
        _objectToActivate[index].SetActive(true);
    }
}
