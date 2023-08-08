using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _log;

    private bool _previousStopped;

    public void OnMenu()
    {
        GameSettings _gameSettings = GameSettings.Instance;
       
        if (_gameSettings.isWorldStopped && !_previousStopped)
        {
            _previousStopped = true;
        }

        if (_previousStopped)
        {
            _previousStopped = false;
        }

        bool menuOpen = !_log.activeSelf;
        _log.SetActive(menuOpen);

        _gameSettings.isWorldStopped = menuOpen;

        Cursor.lockState = menuOpen ? CursorLockMode.None:CursorLockMode.Locked;
        Cursor.visible = menuOpen;
    }
}
