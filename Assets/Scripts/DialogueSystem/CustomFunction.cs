
using System;

namespace DialogueSystem 
{
    public abstract class CustomFunction
    {
        public string GUID { get; set; }
        public Action onUpdate { get; set; }
        public virtual void OnEnable() { }
        public abstract void Draw();
  
    }
}
