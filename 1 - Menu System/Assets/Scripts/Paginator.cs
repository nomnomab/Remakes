using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuSystem {
    // D94242
    public class Paginator: MonoBehaviour {
        public static Paginator Instance { get; private set; }
        
        public int Count => _menus.Count;
        public bool FlowVisible { get; private set; }
        
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Image _backArrow;
        [SerializeField] private Button _backArrowBtn;
        private List<Menu> _menus;
        private bool _flowing;

        private void Awake() {
            if (Instance != null) {
                return;
            }

            Instance = this;
            
            _menus = new List<Menu>();
            _backArrow.enabled = false;
            _backArrowBtn.onClick.AddListener(() => {
                Menu.Current.Cleanup();
                Pop().gameObject.SetActive(false);
                Peek().gameObject.SetActive(true);
                Peek().Animate();
            });
            _label.enabled = false;
        }

        public void Push(Menu menu) {
            if (_menus.Contains(menu)) {
                return;
            }
            
            _menus.Add(menu);
            Repaint();
        }

        public Menu Pop() {
            Menu menu = _menus[_menus.Count - 1];
            _menus.RemoveAt(_menus.Count - 1);
            Repaint();
            return menu;
        }

        public void RunActionOnAll(Action<Menu> action) {
            foreach (Menu menu in _menus) {
                action.Invoke(menu);
            }
        }

        public Menu Peek() {
            return _menus[_menus.Count - 1];
        }

        public void Repaint() {
            if (Count <= 1) {
                _label.enabled = _backArrow.enabled = false;
                FlowVisible = false;
                return;
            }

            _label.enabled = _backArrow.enabled = true;

            Menu menu = Peek();
            _label.text = $"<color=#D94242>{menu.Title}</color>\n{menu.Subtitle}";
            FlowVisible = true;
        }
    }
}