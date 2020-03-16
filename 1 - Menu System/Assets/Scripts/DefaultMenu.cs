using System;
using Attributes;
using UnityEngine;

namespace MenuSystem {
    public class DefaultMenu: CustomBehaviour {
        [SerializeField] private Menu _menu;
        [Require] private Paginator _paginator;

        private void Start() {
            _paginator.Push(_menu);
            _menu.gameObject.SetActive(true);
        }
    }
}