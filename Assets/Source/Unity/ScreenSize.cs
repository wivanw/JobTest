using System;
using UnityEngine;
using Zenject;

namespace Manager
{
    public class ScreenSize : ITickable, IScreenSize
    {
        public Vector2 Size { get; private set; }
        public event Action<Vector2> ChangeSizeEvent;

        public ScreenSize()
        {
            NewSize();
        }
        
        public void Tick()
        {
            if (Math.Abs(Size.y - Screen.height) > float.Epsilon || Math.Abs(Size.x - Screen.width) > float.Epsilon)
            {
                NewSize();
            }
        }

        private void NewSize()
        {
            Size = new Vector2(Screen.width, Screen.height);
            ChangeSizeEvent?.Invoke(Size);
        }
    }
}