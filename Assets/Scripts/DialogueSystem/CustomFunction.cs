
using System;

namespace DialogueSystem 
{
    public abstract class CustomFunction
    {
        public Action onUpdate { get; set; }
        public virtual void OnEnable() { }
        public abstract void Draw();
    }
}
