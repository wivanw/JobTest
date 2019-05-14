using System;
using Data;
using UnityEngine;
using Views;

namespace Game.Infrastructure
{
    public class SceneBorderView : InfrastructureView, ISceneBorderView
    {
        [SerializeField]
        private BoxCollider2D _left;
        [SerializeField]
        private BoxCollider2D _right;
        [SerializeField]
        private BoxCollider2D _top;

        public void SetParams(BorderParamContainer parameters, Data.Enum.Border type)
        {
            switch (type)
            {
                case Data.Enum.Border.Left:
                    _left.offset = parameters.Offset;
                    _left.size = parameters.Size;
                    break;
                case Data.Enum.Border.Right:
                    _right.offset = parameters.Offset;
                    _right.size = parameters.Size;
                    break;
                case Data.Enum.Border.Top:
                    _top.offset = parameters.Offset;
                    _top.size = parameters.Size;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}