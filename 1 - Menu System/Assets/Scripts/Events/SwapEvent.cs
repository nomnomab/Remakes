using UnityEngine;

namespace MenuSystem.Events {
    public class SwapEvent: MenuEvent {
        [SerializeField] private Menu _outcome;
        
        public override void Run() {
            Menu.Current.Cleanup();
            
            Paginator.Instance.Peek().gameObject.SetActive(false);
            Paginator.Instance.Push(_outcome);
            
            _outcome.gameObject.SetActive(true);
            _outcome.Animate();
        }
    }
}