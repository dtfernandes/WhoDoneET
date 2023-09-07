using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/GameSettings")]
public class GameSettings : ScriptableSingletonObject<GameSettings>
{
    [field:SerializeField]
    public bool isWorldStopped { get; set; }

    [field: SerializeField]
    public bool isMenuOpen { get; set; }

    public DialogueDisplayHandler DialogueHandler { get; internal set; }
    [field: SerializeField]
    public bool FirstLoad { get; internal set; }

    public CursorLockMode SavedCursorMode { get; private set; }
    public bool SavedCursorVisibility { get; private set; }

    [field:SerializeField]
    public InvestigationLog Log { get; private set; }

    public List<string> Peoples { get; set; }
    public List<string> Weapons { get; set; }

    public List<string> Selected { get; set; }

    public void Submit(string p, string w, string m)
    {
        Selected[0] = p;
        Selected[1] = w;
        Selected[2] = m;
    }

    public List<string> Motives { get; set; }
    public bool HasNotif { get; set; }
    public string NotifMessage { get; set; }

    public void Start()
    {
        FirstLoad = true;
        isMenuOpen = false;
        isWorldStopped = false;
        Log = new InvestigationLog();

        Selected = new List<string> { "","","" };
        Peoples = new List<string> { "No One" };
        Weapons = new List<string> { "Nothing" };
        Motives = new List<string> { "Just Cause" };
    }

    public void LockCursor(bool state, bool save = true)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;

        if (save)
        {
            SavedCursorVisibility = !Cursor.visible;
            SavedCursorMode = Cursor.lockState;
        }

    }
}
