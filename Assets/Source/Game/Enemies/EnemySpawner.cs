using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Core.Pool;
using Data.Enum;
using EventHabs;
using Game.Enemies;
using Manager;
using Mono;
using UnityEngine;
using Views.Enemy;
using Random = UnityEngine.Random;

namespace Source.Game.Model
{
    public class EnemySpawner : IEnemySpawner, GameRessetEvent.IHandler
    {
        private readonly UnityPoolManager<Enemy, EnemyView> _enemyPool;
        private readonly AsyncProcessor _asyncProcessor;
        private readonly Settings _settings;
        private readonly Camera _camera;
        private readonly IElementSize _size;
        private Vector3 _leftSpawnPos;
        private Vector3 _rightSpawnPos;
        private float _dieHeight;
        private readonly WaitForSeconds _yiledHalfSecond = new WaitForSeconds(0.5f);
        private readonly List<EnemyView> _activeEnemies = new List<EnemyView>();

        public event Action<EnemyView> SpawnEvent;

        public EnemySpawner(UnityPoolManager<Enemy, EnemyView> enemyPool, AsyncProcessor asyncProcessor,
            Settings settings, IScreenSize screenSize, Camera camera, IElementSize size)
        {
            _enemyPool = enemyPool;
            _asyncProcessor = asyncProcessor;
            _settings = settings;
            _camera = camera;
            _size = size;
            screenSize.ChangeSizeEvent += ScreenSizeChange;
            ScreenSizeChange(screenSize.Size);
            asyncProcessor.StartCoroutine(SpawnPoint());
            asyncProcessor.StartCoroutine(SpawnDamage());
        }

        private void ScreenSizeChange(Vector2 size)
        {
            _leftSpawnPos = _camera.ScreenToWorldPoint(new Vector3(0.0f, size.y, -_camera.transform.position.z)) +
                            Vector3.up * 5.0f;
            _rightSpawnPos = _camera.ScreenToWorldPoint(new Vector3(size.x, size.y, -_camera.transform.position.z)) +
                             Vector3.up * 5.0f;
            _dieHeight = _camera.ScreenToWorldPoint(Vector3.zero).y - 5;
            foreach (var activeEnemy in _activeEnemies)
            {
                SetSize(activeEnemy);
            }
        }

        private IEnumerator SpawnPoint()
        {
            while (true)
            {
                var time = Random.Range(_settings.MinPointSpawnFrequency, _settings.MaxPointSpawnFrequency);
                yield return new WaitForSeconds(time);
                var pos = RandPos();
                var pointView = _enemyPool.Pop(Enemy.Point, pos, Quaternion.identity);
                _activeEnemies.Add(pointView);
                pointView.DieEvent += DieEventHandler;
                _asyncProcessor.StartCoroutine(Die(pointView));
                SetSize(pointView);
                SpawnEvent?.Invoke(pointView);
            }
        }

        private void DieEventHandler(EnemyView enemyView)
        {
            enemyView.DieEvent -= DieEventHandler;
            _activeEnemies.Remove(enemyView);
        }

        private IEnumerator SpawnDamage()
        {
            while (true)
            {
                var time = Random.Range(_settings.MaxDamageSpawnFrequency, _settings.MaxDamageSpawnFrequency);
                yield return new WaitForSeconds(time);
                var pos = RandPos();
                var damageView = _enemyPool.Pop(Enemy.Damage, pos, Quaternion.identity);
                _activeEnemies.Add(damageView);
                damageView.DieEvent += DieEventHandler;
                _asyncProcessor.StartCoroutine(Die(damageView));
                SetSize(damageView);
                SpawnEvent?.Invoke(damageView);
            }
        }

        private void SetSize(EnemyView view)
        {
            view.SetSize(_size.SizeFactor);
            view.SetSpeed(_size.SpeedFactor);
        }

        private IEnumerator Die(EnemyView view)
        {
            while (_dieHeight < view.Transform.position.y)
            {
                yield return _yiledHalfSecond;
            }
            view.Die();
        }

        private Vector3 RandPos()
        {
            var rndX = Random.Range(_leftSpawnPos.x, _rightSpawnPos.x);
            var pos = new Vector3(rndX, _leftSpawnPos.y, _leftSpawnPos.z);
            return pos;
        }

        [Serializable]
        public class Settings
        {
            [Range(0.2f, 3.0f)] public float MinPointSpawnFrequency;
            [Range(0.2f, 3.0f)] public float MaxPointSpawnFrequency;
            [Range(0.2f, 3.0f)] public float MinDamageSpawnFrequency;
            [Range(0.2f, 3.0f)] public float MaxDamageSpawnFrequency;
        }

        public object GetCallBackValue(object sendValue)
        {
            foreach (var activeEnemy in _activeEnemies.ToList())
            {
                activeEnemy.Die();
            }
            return null;
        }
    }
}