using UnityEngine;

namespace MenuSystem.Events {
    public class HideRightPanelEvent: InactiveEvent {
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _subPanel;
        
        public override void Run() {
            _panel.SetActive(false);
            _subPanel.SetActive(false);
        }
    }
}