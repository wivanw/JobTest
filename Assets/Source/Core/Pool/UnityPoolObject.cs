using System;
using Core.Pool.Interfaces;
using Mono;

namespace Core.Pool
{
    public class UnityPoolObject<TEnum> : View<TEnum>, IPoolObject<TEnum>
        where TEnum : struct, IConvertible
    {
        public virtual TEnum Group => Type;

        public IPush<TEnum> UnityPoolManager { private get; set; }

        public virtual void OnPop() // Constructor for the pool
        {
            gameObject.SetActive(true);
        }

        public virtual void OnPush() // Destructor for the pool
        {
            gameObject.SetActive(false);
        }

        /// <inheritdoc />
        public virtual void Push()
        {
            UnityPoolManager.Push(this);
        }
    }
}