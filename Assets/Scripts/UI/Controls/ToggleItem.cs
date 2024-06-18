using System;
using UnityEngine;

namespace UI.Controls
{
    public class ToggleItem : MonoBehaviour
    {
        public event Action<bool> Changed;
    
        [SerializeField] private UIToggle _toggle;

        public bool IsOn
        {
            set
            {
                SetValue(value);
                Changed?.Invoke(value);
            }
            get => _toggle.IsOn;
        }

        private void Awake()
        {
            _toggle.Changed += value => IsOn = value;
        }

        public void SetValue(bool value)
        {
            _toggle.IsOn = value;
        }
    }
}