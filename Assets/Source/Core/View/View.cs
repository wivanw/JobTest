using System;
using Manager.Interfaces;
using UnityEngine;

namespace Mono
{
    public abstract class View<TEnum> : MonoBehaviour, IView<TEnum>
        where TEnum : struct, IConvertible
    {
        [SerializeField] private TEnum _type;
        public TEnum Type => _type;
        private Transform _myTransform;
        public Transform Transform => _myTransform ? _myTransform : (_myTransform = GetComponent<Transform>());
        public event Action<string, object> ViewEvent;

        protected void OnViewEvent(string name, object obj)
        {
            ViewEvent?.Invoke(name, obj);
        }
    }
}