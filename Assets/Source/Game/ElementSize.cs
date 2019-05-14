using System;
using Game.Enemies;
using Manager;
using UnityEngine;

namespace Game
{
    public class ElementSize : IElementSize
    {
        private readonly Settings _settings;
        private static readonly Vector2 Factor = new Vector2(880.0f, 320.0f);
        public float SpeedFactor { get; private set; }
        public float SizeFactor { get; private set; }

        public ElementSize(Settings settings, IScreenSize screenSize)
        {
            _settings = settings;
            screenSize.ChangeSizeEvent += CalcNewSize;
            SpeedFactor = _settings.ElementSpeed;
            CalcNewSize(screenSize.Size);
        }

        private void CalcNewSize(Vector2 screenSize)
        {
            SizeFactor = screenSize.x * _settings.ElementSize / Factor.x;
        }

        [Serializable]
        public class Settings
        {
            [Range(0.25f, 3.0f)] public float ElementSpeed;
            [Range(0.25f, 2.0f)] public float ElementSize;
        }
    }
}