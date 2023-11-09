using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopTutorialHelper : MonoBehaviour
{
    [SerializeField]
    private TutorialManager _manager;

    [SerializeField]
    private FadeInScreen fadeScreen;

    public void OnFirstTalkStart()
    {
        _manager.TalkToCopFirst();
    }

    public void OnFirstTalkEnd()
    {
        _manager.TalkCopFirstEnd();
    }

    public void StarFadeOut()
    {
        fadeScreen.SetActive();
        fadeScreen.StartFadeOut();
    }
}
