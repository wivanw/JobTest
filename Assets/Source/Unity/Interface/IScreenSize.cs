using System;
using UnityEngine;

namespace Manager
{
    public interface IScreenSize
    {
        Vector2 Size { get; }
        event Action<Vector2> ChangeSizeEvent;
    }
}