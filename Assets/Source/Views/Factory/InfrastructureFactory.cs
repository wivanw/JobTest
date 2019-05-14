using System.Collections.Generic;
using Data.Enum;
using Manager.Interfaces;
using Mono;
using Views;
using Object = UnityEngine.Object;

namespace View.Factory
{
    public class InfrastructureFactory : IInfrastructureFactory
    {
        private readonly Dictionary<Infrastructure, InfrastructureView> _infrastructureViews =
            new Dictionary<Infrastructure, InfrastructureView>();

        public InfrastructureFactory(InfrastructureView[] infrastructurePrefabs)
        {
            foreach (var gameElementView in infrastructurePrefabs)
            {
                _infrastructureViews.Add(gameElementView.Type, gameElementView);
            }
        }

        public InfrastructureView Cteate(Infrastructure element)
        {
            var prefab = _infrastructureViews[element];
            var view = Object.Instantiate(prefab);
            return view;
        }
    }
}