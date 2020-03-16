using System;
using DG.Tweening;
using MenuSystem.Events;
using UnityEngine;

namespace MenuSystem {
    public class Menu: MonoBehaviour {
        public static Menu Current { get; private set; }

        public string Title;
        public string Subtitle;
        
        [SerializeField] private RectTransform _root;
        [SerializeField] private float _cooldownTime = 0.1f;

        private MenuItem[] _items;
        private int _activeIndex = -1;
        private Vector2 _input;
        private float _time;
        private bool _waiting;
        private static bool _keyToggle;

        private static Tweener _tweenDown;
        private static bool _toggled;
        private float _lastLayer;

        private void Awake() {
            _items = new MenuItem[_root.childCount];
            
            for (int i = 0; i < _items.Length; i++) {
                MenuItem item = _root.GetChild(i).GetComponent<MenuItem>();
                item.Index = i;
                _items[i] = item;
            }

            if (_tweenDown != null) {
                return;
            }
            
            _tweenDown = transform.parent.DOLocalMoveY(-60, 0.2f);
            _tweenDown.SetAutoKill(false);
            _tweenDown.SetRelative(true);
            _tweenDown.Pause();
        }

        private void OnEnable() {
            Current = this;
            _activeIndex = -1;
            SetActive(0);
        }

        public void SetActive(int index) {
            if (index == _activeIndex) {
                return;
            }
            
            if (index > _items.Length - 1) {
                index = 0;
            } else if (index < 0) {
                index = _items.Length - 1;
            }

            if (!_items[index].ActiveInScene) {
                SetActive(index < _activeIndex ? index - 1 : index + 1);
                return;
            }

            _activeIndex = index;

            for (int i = 0; i < _items.Length; i++) {
                bool active = i == _activeIndex;
                bool exiting = _items[i].ActiveIndex;
                
                if (active) {
                    _items[i].GetComponent<ActiveEvent>()?.Run();
                } else {
                    _items[i].GetComponent<InactiveEvent>()?.Run();
                }

                _items[i].SetActive(active);
            }
        }

        private void Update() {
            if (Paginator.Instance.Peek() != this) {
                return;
            }
            
            _input.y = Input.GetAxisRaw("Vertical");

            int y = -(int) _input.y;
            
            if (_waiting) {
                if (Time.time - _time < _cooldownTime && y != 0) {
                    return;
                }

                _waiting = false;
            }
            
            if (y != 0) {
                SetActive(_activeIndex + y);
                _time = Time.time;
                _waiting = true;
            }

            if (_keyToggle) {
                if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Escape)) {
                    _keyToggle = false;
                }
                return;
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                // run action
                _items[_activeIndex].Run();
                _keyToggle = true;
            } else if (Input.GetKeyUp(KeyCode.Escape) && Paginator.Instance.Count > 1) {
                // go back to previous menu
                Cleanup();
                Paginator.Instance.Pop().gameObject.SetActive(false);
                Paginator.Instance.Peek().gameObject.SetActive(true);
                Paginator.Instance.Peek().Animate();
                _keyToggle = true;
            }
        }

        public void Cleanup() {
            foreach (MenuItem menuItem in _items) {
                menuItem.GetComponent<InactiveEvent>()?.Run();
            }
        }
        
        // Events

        public void Animate() {
            bool same = _toggled == Paginator.Instance.FlowVisible;
            
            if (Paginator.Instance.FlowVisible) {
                if (!same) {
                    _tweenDown.Pause();
                    _tweenDown.Rewind();
                    _tweenDown.PlayForward();
                    _toggled = true;
                }
            }
            else {
                if (!same) {
                    _tweenDown.Pause();
                    _tweenDown.Complete();
                    _tweenDown.PlayBackwards();
                    _toggled = false;
                }
            }
        }
    }
}