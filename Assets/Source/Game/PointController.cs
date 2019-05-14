using Data.Enum;
using EventHabs;

namespace Game
{
    public class PointController : ChangePointEvent.IHandler, IPointController, GameRessetEvent.IHandler
    {
        private readonly IPointModel _pointModel;

        public PointController(IPointModel pointModel)
        {
            _pointModel = pointModel;
        }
        
        public int GetCallBackValue(int sendValue)
        {
            return _pointModel.Point;
        }

        public void EnemyFaced(Enemy enemy)
        {
            switch (enemy)
            {
                    case Enemy.Point:
                        _pointModel.AddPoint();
                        break;
                    case Enemy.Damage:
                        _pointModel.RemovePoint();
                        break;
            }
        }

        public object GetCallBackValue(object sendValue)
        {
            _pointModel.Resset();
            return null;
        }
    }
}