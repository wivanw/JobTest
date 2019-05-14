using Common;
using Core.Pool;
using Data;
using Data.Enum;
using EventHabs;
using Game;
using Game.Enemies;
using Game.Infrastructure;
using Manager;
using Manager.Interfaces;
using Source.Game.Infrastructure;
using Source.Game.Model;
using UI;
using UnityEngine;
using VBCM;
using View.Factory;
using Views;
using Views.Enemy;
using Zenject;
using Border = Game.Infrastructure.Border;

public class MainInstaller : MonoInstaller
{
    //todo need divide by ScriptableObject
    [SerializeField] private Settings _settings;
    [SerializeField] private Resources _resources;
    [SerializeField] private Transform _poolRoot;
    [SerializeField] private Camera _camera;
    [SerializeField] private BarInfoView _barInfoView;
    [SerializeField] private InputView _inputView;


    public override void InstallBindings()
    {
        var obj = new GameObject("AsyncProcessor");
        Container.Bind<AsyncProcessor>().FromNewComponentOnNewPrefab(obj).AsSingle();
        VbcmInstaller.Install(Container);
        Container.Bind(typeof(IInputManager), typeof(ITickable)).To<InputManager>().AsSingle();
        Container.Bind(typeof(IScreenSize), typeof(ITickable)).To<ScreenSize>().AsSingle();
        Container.BindInstance(_camera);
        Pools();
        Factory();
        Events();
        //Infrastructure===========================================
        Container.Bind<IBottom>().To<Bottom>().AsSingle();
        Container.Bind<GameRessetEvent.IHandler>().To<BottomPresenter>().AsSingle().NonLazy();
        Container.BindInstance(_settings.BottomSettings);

        Container.Bind<IBorder>().To<Border>().AsSingle();
        Container.Bind<BorderPresenter>().AsSingle().NonLazy();

        Container.Bind<GameRessetEvent.IHandler>().To<BallPresenter>().AsSingle().NonLazy();
        //Enemy
        Container.BindInstance(_settings.EnemySpawnerSettings).AsSingle();
        Container.Bind(typeof(IEnemySpawner), typeof(GameRessetEvent.IHandler)).To<EnemySpawner>().AsSingle();
        Container.Bind<EnemyMediator>().AsSingle().NonLazy();
        Container.Bind<IElementSize>().To<ElementSize>().AsSingle();
        Container.BindInstance(_settings.EnemySizeSettings).AsSingle();
        //Logyc
        Container.Bind(typeof(IPointController), typeof(ChangePointEvent.IHandler),
                typeof(GameRessetEvent.IHandler)).To<PointController>()
            .AsSingle();
        Container.Bind(typeof(IPointModel), typeof(ChangePointEvent.IValidated), typeof(ChangePointEvent.IEventSource))
            .To<PointModel>().AsSingle();
    }

    private void Pools()
    {
        //EnemyView===========================================
        Container.Bind<UnityPoolManager<Enemy, EnemyView>>().AsSingle().WithArguments(_poolRoot);
        Container.Bind<UnityPoolManager<Enemy, EnemyView>.Factory>().AsSingle()
            .WithArguments(_resources.EnemyPrefabs);
    }

    private void Factory()
    {
        //InfrastructureView===========================================
        Container.BindInstance(_resources.InfrastructurePrefabs).AsSingle();
        Container.Bind<IInfrastructureFactory>().To<InfrastructureFactory>().AsSingle();
    }

    private void Events()
    {
        Container.Bind<ChangePointEvent>().AsSingle().NonLazy();
        Container.Bind(typeof(ChangePointEvent.IBindable)).FromInstance(_barInfoView).AsSingle();
        Container.Bind<GameRessetEvent>().AsSingle().NonLazy();
        Container.Bind(typeof(GameRessetEvent.IBindable), typeof(GameRessetEvent.IEventSource)).FromInstance(_inputView)
            .AsSingle();
    }
}