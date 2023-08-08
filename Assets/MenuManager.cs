using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _log;

    public void OnMenu()
    {
        GameSettings _gameSettings = GameSettings.Instance;

        bool menuOpen = !_log.activeSelf;

        _log.SetActive(menuOpen);

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
}
