using Manager;
using UnityEngine;
using View.Factory;

namespace Game.Infrastructure
{
    public class BorderPresenter
    {
        private readonly IBorder _border;
        private readonly ISceneBorderView _view;

        public BorderPresenter(IInfrastructureFactory infrastructureFactory, IBorder border, IScreenSize screenSize)
        {
            _border = border;
            _view = infrastructureFactory.Cteate(Data.Enum.Infrastructure.Border) as ISceneBorderView;
            screenSize.ChangeSizeEvent += ChangeSizeEventHandler;
            ChangeSizeEventHandler(screenSize.Size);
        }

        private void ChangeSizeEventHandler(Vector2 size)
        {
            var leftParams = _border.GetParams(size, Data.Enum.Border.Left);
            var rightParams = _border.GetParams(size, Data.Enum.Border.Right);
            var topParams = _border.GetParams(size, Data.Enum.Border.Top);
            _view.SetParams(leftParams, Data.Enum.Border.Left);
            _view.SetParams(rightParams, Data.Enum.Border.Right);
            _view.SetParams(topParams, Data.Enum.Border.Top);
        }
    }
}