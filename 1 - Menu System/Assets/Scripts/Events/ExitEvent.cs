using UnityEngine;

namespace MenuSystem.Events {
    public class ExitEvent: MenuEvent {
        public override void Run() {
            Application.Quit();
        }
    }
}