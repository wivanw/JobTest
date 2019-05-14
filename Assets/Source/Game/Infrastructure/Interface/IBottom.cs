using UnityEngine;

namespace Source.Game.Model
{
    public interface IBottom
    {
        float CulcBottomPivot(Vector2 screenSize);
        Vector2 CulcBottomSize(Vector2 viewPivot);
        void StartDrag();
        float Drag();
        void EndDrag();
        void Resset();
    }
}