using System;
using Data;
using UnityEngine;

namespace Game.Infrastructure
{
    public class Border : IBorder
    {
        private readonly Camera _camera;

        public Border(Camera camera)
        {
            _camera = camera;
        }

        public BorderParamContainer GetParams(Vector2 size, Data.Enum.Border type)
        {
            var topRightPos = _camera.ScreenToWorldPoint(size);
            BorderParamContainer parameters;
            switch (type)
            {
                case Data.Enum.Border.Left:
                    var leftPos = _camera.ScreenToWorldPoint(Vector3.zero);
                    parameters = new BorderParamContainer
                    {
                        Size = new Vector2(1.0f, topRightPos.y * 2.0f),
                        Offset = new Vector2(leftPos.x - 0.5f, 0.0f)
                    };
                    break;
                case Data.Enum.Border.Right:
                    parameters = new BorderParamContainer
                    {
                        Size = new Vector2(1.0f, topRightPos.y * 2.0f),
                        Offset = new Vector2(topRightPos.x + 0.5f, 0.0f)
                    };
                    break;
                case Data.Enum.Border.Top:
                    parameters = new BorderParamContainer
                    {
                        Size = new Vector2(topRightPos.x * 2.0f, 1.0f),
                        Offset = new Vector2(0.0f, topRightPos.y + 0.5f)
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return parameters;
        }
    }
}