using Manager.Interfaces;
using UnityEngine;

namespace Views
{
    public interface IBottomView : IView<Data.Enum.Infrastructure>
    {
        void SetIncline(float angle);
        void SetSize(Vector2 size);
    }
}