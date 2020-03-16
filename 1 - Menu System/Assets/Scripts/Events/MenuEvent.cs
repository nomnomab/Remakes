using UnityEngine;

namespace MenuSystem.Events {
    public abstract class MenuEvent: MonoBehaviour {
        public abstract void Run();
    }
}