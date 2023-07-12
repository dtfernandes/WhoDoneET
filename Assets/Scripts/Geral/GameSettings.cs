using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/GameSettings")]
public class GameSettings : ScriptableSingletonObject<GameSettings>
{
    [field:SerializeField]
    public bool isWorldStopped { get; set; }
}
