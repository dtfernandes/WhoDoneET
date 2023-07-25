using UnityEngine;

public class Loader : MonoBehaviour
{

    [SerializeReference]
    private DialogueDisplayHandler _dHandler;

    // Start is called before the first frame update
    void Awake()
    {
        GameSettings.Instance.DialogueHandler = _dHandler;
    }
}
