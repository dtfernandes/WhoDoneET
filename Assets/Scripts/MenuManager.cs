using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private LogDisplay _log;
    
    [SerializeField]
    private LogDisplay _miniLog;

    public void OnMenu()
    {
        GameObject log = _log.gameObject;

        GameSettings _gameSettings = GameSettings.Instance;

        bool menuOpen = !log.activeSelf;

        log.SetActive(menuOpen);

        _gameSettings.isMenuOpen = menuOpen;

        if (menuOpen)
        {
            _gameSettings.LockCursor(false,false);
        }
        else
        {
            _gameSettings.LockCursor(_gameSettings.SavedCursorVisibility);
        }
       
    }

    public void OnInspect()
    {

        GameObject miniLog = _miniLog.gameObject;

        GameSettings _gameSettings = GameSettings.Instance;

        if(_gameSettings.isMenuOpen) return;

        //Only opens when in dialogue,
        if(!_gameSettings.DialogueHandler.InDialogue) return;

        miniLog.SetActive(!miniLog.activeSelf);

        _miniLog.GoBack(true);

        // Stops the Dialogue while opens 
        _gameSettings.DialogueHandler.DialogueIsPaused = miniLog.activeSelf;
        
    }

}
