using System;
using System.Collections.Generic;
using Data;
using EventHabs;
using Manager;
using Source.Game.Model;
using UnityEngine;
using View.Factory;
using Views;

namespace Source.Game.Infrastructure
{
    public class BottomPresenter : GameRessetEvent.IHandler
    {
        private readonly IBottomView _view;
        private readonly IBottom _model;
        private readonly Dictionary<string, Action> _bottomEventActions;

        public BottomPresenter(IInfrastructureFactory infrastructureFactory, IBottom bottom, IScreenSize screenSize)
        {
            _view = infrastructureFactory.Cteate(Data.Enum.Infrastructure.Bottom) as IBottomView;
            _model = bottom;
            _view.ViewEvent += ViewEventHandler;
            screenSize.ChangeSizeEvent += ChangeSizeEventHandler;
            ChangeSizeEventHandler(screenSize.Size);
            _bottomEventActions = new Dictionary<string, Action>()
            {
                {BottomEvent.ButtonDown, () => _model.StartDrag()},
                {BottomEvent.Drag, () => _view.SetIncline(_model.Drag())},
                {BottomEvent.ButtonUp, () => _model.EndDrag()}
            };
        }

        private void ChangeSizeEventHandler(Vector2 screenSize)
        {
            var pivot = _view.Transform.position;
            var pivotHeight = _model.CulcBottomPivot(screenSize);
            pivot.y = pivotHeight;
            var sizeBottom = _model.CulcBottomSize(pivot);
            _view.Transform.position = pivot;
            _view.SetSize(sizeBottom);
        }

        private void ViewEventHandler(string name, object axis)
        {
            _bottomEventActions[name].Invoke();
        }

        public object GetCallBackValue(object sendValue)
        {
            _view.SetIncline(0.0f);
            _model.Resset();
            return null;
        }
    }
}