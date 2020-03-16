using UnityEngine;

namespace MenuSystem.Events {
    public class ShowRightPanelEvent: ActiveEvent {
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _subPanel;
        
        public override void Run() {
            _panel.SetActive(true);
            _subPanel.SetActive(true);
        }
    }
}