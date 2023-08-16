using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/EntityDisplayData")]
public class EntiyDisplayData : ScriptableObject
{
    [field:SerializeField]
    public string Name {get; private set; }

    [field:SerializeField]
    public Sprite Image { get; private set; }
}
