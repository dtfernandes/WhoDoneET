using DialogueSystem;
using UnityEngine;
using TMPro;

public class NotifDisplay : MonoBehaviour
{

    [SerializeField]
    private DialogueDisplayHandler _dialogueDisplay;

    private Animator _anim;

    [SerializeField]
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _dialogueDisplay.onEndDialogue += (e) =>
        {
            TriggerNotif(e);
        };
    }

    public void TriggerNotif(IDialogueScript script)
    {
        if (GameSettings.Instance.HasNotif)
        {
            _anim.Play("TriggerNotif");
            GameSettings.Instance.HasNotif = false;
            string message = GameSettings.Instance.NotifMessage;
            _text.text = "New " + message + " Log";
        }
    }
}
