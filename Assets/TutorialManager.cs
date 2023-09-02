using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using System;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tutMarkerCop, tutMarkerCup;
    [SerializeField]
    private DController copContr;

    public void TalkToCopFirst()
    {
        tutMarkerCop.SetActive(false);
    }

    public void TalkCopFirstEnd()
    {
        
        tutMarkerCup.SetActive(true);
        copContr.ChangeDefault(1);
    }

    internal void CupEnd()
    {
        tutMarkerCop.SetActive(true);
        tutMarkerCup.SetActive(false);
    }
}
