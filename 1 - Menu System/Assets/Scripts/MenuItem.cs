using System;
using Attributes;
using DG.Tweening;
using MenuSystem.Events;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MenuSystem {
    public class MenuItem: CustomBehaviour, IPointerEnterHandler {
        public bool ActiveInScene = true;
        public bool ActiveIndex { get; private set; }
        public int Index;
        
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Image _bg;
        [SerializeField] private Image _outline;
        [Require] private MenuEvent _event;
        [RequireChild(0)] private Button _button;
        
        private const float _time = 0.5f;

        private void Start() {
            Repaint();
            
            _button.onClick.AddListener(Run);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            Menu.Current.SetActive(Index); 
        }

        public void SetActive(bool active) {
            ActiveIndex = active;
            
            Repaint();
        }

        public void Repaint() {
            _bg.enabled = _outline.enabled = ActiveIndex;
            
            if (!ActiveInScene) {
                Color c = _bg.color;
                c.a = 0.95f;
                _bg.color = c;
                _label.color = c;
            }
        }

        public void Run() {
            _event?.Run();
        }

        public void FadeOut() {
            _bg.DOColor(Color.clear, _time);
            _outline.DOColor(Color.clear, _time - 0.4f);
            _label.DOColor(Color.clear, _time);
        }

        public void FadeIn() {
            _bg.DOColor(Color.white, _time);
            _outline.DOColor(Color.white, _time + 0.4f);
            _label.DOColor(Color.white, _time);
        }
    }
}