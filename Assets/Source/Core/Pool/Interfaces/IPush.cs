using System;

namespace Core.Pool.Interfaces
{
    public interface IPush<TEnum>
        where TEnum : struct, IConvertible
    {
        void Push(UnityPoolObject<TEnum> obj);
    }
}