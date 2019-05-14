using System;
using System.Collections.Generic;
using System.Linq;
using Core.Pool.Interfaces;
using UnityEngine;
using Zenject;

namespace Core.Pool
{
    public class UnityPoolManager<TEnum, TComponent> : IPush<TEnum>
        where TComponent : UnityPoolObject<TEnum>
        where TEnum : struct, IConvertible
    {
        private readonly Dictionary<TEnum, List<TComponent>> _objects =
            new Dictionary<TEnum, List<TComponent>>(EnumComparer);

        private readonly Factory _factory;
        private readonly Transform _root;

        public UnityPoolManager(Factory factory, Transform root)
        {
            _factory = factory;
            _root = root;
        }

        public void Push(UnityPoolObject<TEnum> obj)
        {
            Push((TComponent) obj);
        }

        public void Push(TComponent value)
        {
            value.OnPush();
            value.Transform.parent = _root;
            List<TComponent> list;
            if (!_objects.TryGetValue(value.Group, out list))
                list = new List<TComponent>();

            if (!list.Contains(value))
                list.Add(value);
            _objects[value.Group] = list;
        }

        public TComponent Pop(TEnum type)
        {
            return Pop(type, Vector3.zero, Quaternion.identity);
        }

        public TComponent Pop(TEnum type, Vector3 position, Quaternion rotation, Transform parentTransform = null)
        {
            List<TComponent> list;
            if (!_objects.TryGetValue(type, out list))
                list = new List<TComponent>();

            TComponent component;
            if (list.Count > 0)
            {
                component = list[0];
                list.RemoveAt(0);
                component.Transform.position = position;
                component.Transform.rotation = rotation;
                component.Transform.parent = parentTransform ? parentTransform : _root;
            }
            else
            {
                component = _factory.Create(type, position, rotation, parentTransform ? parentTransform : _root);
                component.UnityPoolManager = this;
            }

            component.OnPop();
            return component;
        }

        public class Factory
        {
            private readonly DiContainer _container;
            private readonly Dictionary<TEnum, TComponent> _prefubs;

            public Factory(TComponent[] prefabs, DiContainer container)
            {
                _container = container;
                _prefubs = prefabs.ToDictionary(c => c.Type);
            }

            public TComponent Create(TEnum type)
            {
                return Create(type, Vector3.zero, Quaternion.identity);
            }

            public TComponent Create(TEnum type, Vector3 position, Quaternion rotation,
                Transform parentTransform = null)
            {
                var transf = _container.InstantiatePrefab(_prefubs[type], position, rotation, parentTransform)
                    .GetComponent<TComponent>();
                return transf;
            }
        }

        private sealed class EnumEqualityComparer : IEqualityComparer<TEnum>
        {
            public bool Equals(TEnum x, TEnum y)
            {
                if (x.GetType() != y.GetType()) return false;
                return x.Equals(y);
            }

            public int GetHashCode(TEnum obj)
            {
                return obj.GetHashCode();
            }
        }

        public static IEqualityComparer<TEnum> EnumComparer { get; } = new EnumEqualityComparer();
    }
}