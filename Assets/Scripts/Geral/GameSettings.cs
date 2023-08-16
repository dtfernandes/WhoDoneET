using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public void Start()
    {
        FirstLoad = true;
        isMenuOpen = false;
        isWorldStopped = false;     
        Log = new InvestigationLog();
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

[System.Serializable]
public class InvestigationLog
{
    [field:SerializeField]
    public List<LogItem> Items { get; private set; }

    public InvestigationLog()
    {
        Items = new List<LogItem> { };
    }

    public void AddItem(LogItem item)
    {

        //Check if the item is already in the log

        if(Items.Any(x=> x.GUID == item.GUID))
        {
            return;
        }


        Items.Add(item);
    }
}
