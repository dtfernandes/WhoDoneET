using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalLoader
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        GameSettings _settings = GameSettings.Instance;
        _settings.FirstLoad = true;
        _settings.isWorldStopped = false;     
    }
}
