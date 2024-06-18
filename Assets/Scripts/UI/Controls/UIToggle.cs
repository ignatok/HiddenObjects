using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controls
{
    public class UIToggle : Button
    {
        public static event Action<bool> StaticClick;

        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;
        [SerializeField] private Image _targetImage;
    
        public event Action<bool> Changed;

        private bool _isOn;
    
        public bool IsOn
        {
            get => _isOn;
            set
            {
                _targetImage.sprite = value ? _onSprite : _offSprite;
            
                if (_isOn == value)
                    return;

                _isOn = value;
                Changed?.Invoke(_isOn);
            }
        }
    
        protected override void Awake()
        {
            base.Awake();
        
            onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            IsOn = !IsOn;
        
            StaticClick?.Invoke(_isOn);
        }
    }
}
