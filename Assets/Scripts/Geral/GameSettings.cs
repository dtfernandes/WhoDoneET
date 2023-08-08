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
