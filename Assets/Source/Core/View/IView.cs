using System;
using UnityEngine;

namespace Manager.Interfaces
{
    public interface IView<out TEnum> where TEnum : struct, IConvertible
    {
        TEnum Type { get; }
        Transform Transform { get; }
        event Action<string, object> ViewEvent;
    }
}