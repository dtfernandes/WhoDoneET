
using System;

namespace DialogueSystem 
{
    [System.Serializable]
    public abstract class CustomFunction
    {
        [field:UnityEngine.SerializeField]
        public string GUID { get; set; }
        public Action onUpdate { get; set; }
        public virtual void OnEnable() { }
        public abstract void Draw();
        public abstract void Invoke();
    }
}
