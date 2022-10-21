using CodeBase.Data.StaticData;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic.Planetary.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States {
  public class BootstrapState : IState {
    private readonly GameStateMachine _stateMachine;
    private readonly AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, AllServices allServices) {
      _stateMachine = stateMachine;
      _services = allServices;
      RegisterServices();
    }

    public void Enter() {
      _stateMachine.Enter<LoadLevelState>();
    }

    public void Exit() { }

    private void RegisterServices() {
      RegisterRandomService();
      IAssetsProvider assetsProvider = RegisterIAssetsProvider();

      RegisterIPlaneterySystemFactory(assetsProvider);
    }

    private void RegisterRandomService() {
      IRandomService randomService = new RandomService();
      randomService.InitState();
      _services.RegisterSingle(randomService);
    }

    private IAssetsProvider RegisterIAssetsProvider() {
      IAssetsProvider assetsProvider = new AssetsProvider();
      _services.RegisterSingle(assetsProvider);
      return assetsProvider;
    }

    private void RegisterIPlaneterySystemFactory(IAssetsProvider assetsProvider) {
      var planetStaticData = assetsProvider.Load<PlanetsStaticData>(AssetsPath.PLANET_STATIC_DATA);
      var planeteryObject = assetsProvider.Load<GameObject>(AssetsPath.PLANET);
      var planeterySystemBehaviour = assetsProvider.Load<GameObject>(AssetsPath.PLANETARY_SYSTEM);
      var orbit = assetsProvider.Load<GameObject>(AssetsPath.ORBIT);
      var planeterySystemCalculator = new PlaneterySystemCalculator(planetStaticData, _services.Single<IRandomService>());
      var planeterySystemFactory = new PlaneterySystemFactory(planeteryObject, planeterySystemBehaviour, orbit, planetStaticData, _services.Single<IRandomService>(),
        planeterySystemCalculator);
      _services.RegisterSingle<IPlaneterySystemFactory>(planeterySystemFactory);
    }
  }
}