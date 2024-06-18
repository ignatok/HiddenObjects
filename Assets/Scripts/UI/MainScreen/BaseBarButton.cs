using UI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainScreen
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseBarButton<T> : ListElement<T>
    {
        private Button _button;

        public virtual void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }
    
        protected abstract void OnClick();
    }
}


