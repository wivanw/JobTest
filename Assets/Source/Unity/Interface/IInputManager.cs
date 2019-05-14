using System;
using UnityEngine;

namespace Manager.Interfaces
{
    public interface IInputManager
    {
        void SubscribeBtn(string btn, Action action);
        void SubscribeBtnDown(string btn, Action action);
        void SubscribeBtnUp(string btn, Action action);
        void UnSubscribeBtn(string btn, Action action);
        void UnSubscribeBtnDown(string btn, Action action);
        void UnSubscribeBtnUp(string btn, Action action);
        void SubscribeAxis(bool isVerticalAxis, Action<float> action);
        void UnSubscribeAxis(bool isVerticalAxis, Action<float> action);
        bool RayCast(out Vector3 position);
    }
}