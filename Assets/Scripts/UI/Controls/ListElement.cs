using UnityEngine;

namespace UI.Controls
{
    public abstract class ListElement<T> : Element
    {
        protected T Data;

        public void SetData(T data)
        {
            Data = data;

            OnSetData(data);
        }

        protected abstract void OnSetData(T data);
    }
    [RequireComponent(typeof(RectTransform))]
    public class Element : MonoBehaviour, IElement
    {
        public RectTransform RectTransform =>
            gameObject.GetComponent<RectTransform>();

        public Transform Transform { get; }
    }
}