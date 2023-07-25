using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/GameSettings")]
public class GameSettings : ScriptableSingletonObject<GameSettings>
{
    [field:SerializeField]
    public bool isWorldStopped { get; set; }
    public DialogueDisplayHandler DialogueHandler { get; internal set; }
    [field: SerializeField]
    public bool FirstLoad { get; internal set; }
}
