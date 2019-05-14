using Data;
using UnityEngine;

namespace Game.Infrastructure
{
    public interface IBorder
    {
        BorderParamContainer GetParams(Vector2 size, Data.Enum.Border type);
    }
}