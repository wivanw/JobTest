using System;
using System.Collections.Generic;
using Data;
using Manager.Interfaces;
using UnityEngine;
using Zenject;

namespace Manager
{
    public class InputManager : ITickable, IInputManager
    {
        private readonly IDictionary<string, List<Action>> _btnEvents = new Dictionary<string, List<Action>>();
        private readonly IDictionary<string, List<Action>> _btnDownEvents = new Dictionary<string, List<Action>>();
        private readonly IDictionary<string, List<Action>> _btnUpEvents = new Dictionary<string, List<Action>>();

        private readonly IDictionary<string, List<Action<float>>> _axisEvents =
            new Dictionary<string, List<Action<float>>>
            {
                {Const.Input.Horizontal, new List<Action<float>>()},
                {Const.Input.Vertical, new List<Action<float>>()}
            };

        private readonly Camera _cameraMain;

        public InputManager()
        {
            _cameraMain = Camera.main;
        }

        public void SubscribeBtn(string btn, Action action)
        {
            Add(_btnEvents, btn, action);
        }

        public void SubscribeBtnDown(string btn, Action action)
        {
            Add(_btnDownEvents, btn, action);
        }

        public void SubscribeBtnUp(string btn, Action action)
        {
            Add(_btnUpEvents, btn, action);
        }

        public void UnSubscribeBtn(string btn, Action action)
        {
            Remove(_btnEvents, btn, action);
        }


        public void UnSubscribeBtnDown(string btn, Action action)
        {
            Remove(_btnDownEvents, btn, action);
        }

        public void UnSubscribeBtnUp(string btn, Action action)
        {
            Remove(_btnUpEvents, btn, action);
        }

        public void SubscribeAxis(bool isVerticalAxis, Action<float> action)
        {
            var list = _axisEvents[isVerticalAxis ? Const.Input.Vertical : Const.Input.Horizontal];
            list.Add(action);
        }

        public void UnSubscribeAxis(bool isVerticalAxis, Action<float> action)
        {
            var list = _axisEvents[isVerticalAxis ? Const.Input.Vertical : Const.Input.Horizontal];
            list.Remove(action);
        }

        private static void Add(IDictionary<string, List<Action>> dictionary, string btn, Action action)
        {
            List<Action> list;
            list = dictionary.TryGetValue(btn, out list) ? list : new List<Action>();
            list.Add(action);
            dictionary[btn] = list;
        }

        private static void Remove(IDictionary<string, List<Action>> dictionary, string btn, Action action)
        {
            List<Action> list;
            list = dictionary.TryGetValue(btn, out list) ? list : new List<Action>();
            list.Remove(action);
            if (list.Count == 0)
                dictionary.Remove(btn);
            else
                dictionary[btn] = list;
        }

        public bool RayCast(out Vector3 position)
        {
            var ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                position = Vector3.zero;
                return false;
            }
            
            position = hit.point;
            return true;
        }

        public void Tick()
        {
            BtnInvoke(Input.GetButton, _btnEvents);
            BtnInvoke(Input.GetButtonDown, _btnDownEvents);
            BtnInvoke(Input.GetButtonUp, _btnUpEvents);

            foreach (var axisEvent in _axisEvents)
            {
                var axis = Input.GetAxis(axisEvent.Key);
                if (Math.Abs(axis) >= float.Epsilon)
                    foreach (var action in axisEvent.Value)
                        action.Invoke(axis);
            }
        }

        private void BtnInvoke(Func<string, bool> btnAction, IDictionary<string, List<Action>> subscribers)
        {
            foreach (var btnEvent in subscribers)
            {
                if (!btnAction(btnEvent.Key)) 
                    continue;
                
                foreach (var action in btnEvent.Value)
                    action.Invoke();
            }
        }
    }
}