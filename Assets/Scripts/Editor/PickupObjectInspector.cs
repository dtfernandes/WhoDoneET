using UnityEngine;
using UnityEditor;


public class PickupObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PickupableObject raycastOnClick = (PickupableObject)target;

        if (GUILayout.Button("Enable Raycast Interaction"))
        {
           
        }
    }
}
