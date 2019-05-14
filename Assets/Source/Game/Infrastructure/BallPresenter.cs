using EventHabs;
using Game.Enemies;
using Manager;
using UnityEngine;
using View.Factory;
using Views;

namespace Game.Infrastructure
{
    public class BallPresenter : GameRessetEvent.IHandler
    {
        private readonly IElementSize _elementSize;
        private readonly IBallView _view;

        public BallPresenter(IInfrastructureFactory infrastructureFactory, IScreenSize screenSize,
            IElementSize elementSize)
        {
            _elementSize = elementSize;
            _view = infrastructureFactory.Cteate(Data.Enum.Infrastructure.Ball) as IBallView;
            screenSize.ChangeSizeEvent += ChangeScreenSize;
        }

        private void ChangeScreenSize(Vector2 obj)
        {
            _view.Resset();
            _view.SetSize(_elementSize.SizeFactor);
        }

        public object GetCallBackValue(object sendValue)
        {
            _view.Resset();
            return null;
        }
    }
}