using Data;
using Source.Game.Model;
using Views.Enemy;

namespace Game.Enemies
{
    public class EnemyMediator
    {
        private readonly IPointController _pointController;

        public EnemyMediator(IEnemySpawner enemySpawner, IPointController pointController)
        {
            _pointController = pointController;
            enemySpawner.SpawnEvent += Spawn;
        }

        private void Spawn(EnemyView enemyView)
        {
            enemyView.ViewEvent += ViewEventHadler;
            enemyView.DieEvent += EnemyDie;
        }

        private void EnemyDie(EnemyView enemyView)
        {
            enemyView.ViewEvent -= ViewEventHadler;
            enemyView.DieEvent -= EnemyDie;
        }

        private void ViewEventHadler(string name, object obj)
        {
            var enemyView = obj as EnemyView;
            if (name == PointViewEvent.BallTrigger)
            {
                _pointController.EnemyFaced(enemyView.Type);
                enemyView.Die();
            }
        }
    }
}