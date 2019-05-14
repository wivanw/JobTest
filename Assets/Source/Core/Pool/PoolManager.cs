using System;
using System.Collections.Generic;
using Core.Pool.Interfaces;

namespace Core.Pool
{
    public class PoolManager<TK, TV> where TV : IPoolObject<TK>
    {
        public delegate bool Compare<in T>(T value) where T : TV;

        private readonly Dictionary<TK, List<TV>> _objects = new Dictionary<TK, List<TV>>();
        private readonly Dictionary<Type, List<TV>> _cache = new Dictionary<Type, List<TV>>();

        public virtual void Push(TK groupKey, TV value)
        {
            value.OnPush();
            if (!_objects.ContainsKey(groupKey))
                _objects.Add(groupKey, new List<TV>());

            _objects[groupKey].Add(value);
            var type = value.GetType();
            if (!_cache.ContainsKey(type))
            {
                _cache.Add(type, new List<TV>());
            }

            _cache[type].Add(value);
        }

        public virtual T Pop<T>(TK groupKey) where T : TV
        {
            var result = default(T);
            if (Contains(groupKey) && _objects[groupKey].Count > 0)
            {
                for (var i = 0; i < _objects[groupKey].Count; i++)
                {
                    if (_objects[groupKey][i] is T)
                    {
                        result = (T) _objects[groupKey][i];
                        var type = result.GetType();
                        RemoveObject(groupKey, i);
                        RemoveFromCache(result, type);
                        result.OnPop();
                        break;
                    }
                }
            }

            return result;
        }

        public virtual T Pop<T>() where T : TV
        {
            var result = default(T);
            var type = typeof(T);
            if (ValidateForPop(type))
            {
                for (var i = 0; i < _cache[type].Count; i++)
                {
                    result = (T) _cache[type][i];
                    if (result != null && _objects.ContainsKey(result.Group))
                    {
                        _objects[result.Group].Remove(result);
                        RemoveFromCache(result, type);
                        result.OnPop();
                        break;
                    }
                }
            }

            return result;
        }

        public virtual T Pop<T>(Compare<T> comparer) where T : TV
        {
            var result = default(T);
            var type = typeof(T);
            if (ValidateForPop(type))
            {
                for (var i = 0; i < _cache[type].Count; i++)
                {
                    var value = (T) _cache[type][i];
                    if (comparer(value))
                    {
                        _objects[value.Group].Remove(value);
                        RemoveFromCache(result, type);
                        result = value;
                        result.OnPop();
                        break;
                    }
                }
            }

            return result;
        }


        public virtual bool Contains(TK groupKey)
        {
            return _objects.ContainsKey(groupKey);
        }

        public virtual void Clear()
        {
            _objects.Clear();
        }

        protected virtual bool ValidateForPop(Type type)
        {
            return _cache.ContainsKey(type) && _cache[type].Count > 0;
        }

        protected virtual void RemoveObject(TK groupKey, int idx)
        {
            if (idx >= 0 && idx < _objects[groupKey].Count)
            {
                _objects[groupKey].RemoveAt(idx);
                if (_objects[groupKey].Count == 0)
                    _objects.Remove(groupKey);
            }
        }

        protected void RemoveFromCache(TV value, Type type)
        {
            if (_cache.ContainsKey(type))
            {
                _cache[type].Remove(value);
                if (_cache[type].Count == 0)
                    _cache.Remove(type);
            }
        }
    }
}