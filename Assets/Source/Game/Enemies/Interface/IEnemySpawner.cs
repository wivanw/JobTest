using System;
using Data.Enum;
using Mono;
using Views.Enemy;

namespace Source.Game.Model
{
    public interface IEnemySpawner
    {
        event Action<EnemyView> SpawnEvent;
    }
}