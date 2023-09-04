using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopTutorialHelper : MonoBehaviour
{
    [SerializeField]
    private TutorialManager _manager;

    public void OnFirstTalkStart()
    {
        _manager.TalkToCopFirst();
    }

    public void OnFirstTalkEnd()
    {
        _manager.TalkCopFirstEnd();
    }
}
