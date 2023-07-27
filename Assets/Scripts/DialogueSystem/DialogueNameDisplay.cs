using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DialogueSystem;

public class DialogueNameDisplay : MonoBehaviour
{
    [SerializeField]
    private DialogueDisplayHandler ddHandler;

    private TextMeshProUGUI textComponent;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        EntityData presetdata = Resources.Load<EntityData>("EntityData");

        ddHandler.onStartLine += (NodeData data) =>
        {
            if (data.PresetName != 0)
                textComponent.text = presetdata.presetNames[data.PresetName];
            else
                textComponent.text = "";
        };
    }

}
