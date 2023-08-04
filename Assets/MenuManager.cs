using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _log;

    public void OnMenu()
    {
        bool menuOpen = !_log.activeSelf;
        _log.SetActive(menuOpen);

        GameSettings.Instance.isWorldStopped = menuOpen;

        Cursor.lockState = menuOpen ? CursorLockMode.None:CursorLockMode.Locked;
        Cursor.visible = menuOpen;
    }
}
