using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifDisplay : MonoBehaviour
{

    [SerializeField]
    private DialogueDisplayHandler _dialogueDisplay;

    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _dialogueDisplay.onEndDialogue += () =>
        {
            TriggerNotif();
        };
    }

    public void TriggerNotif()
    {
        _anim.Play("TriggerNotif");
    }
}
