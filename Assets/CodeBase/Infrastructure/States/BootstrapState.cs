using CodeBase.Data.StaticData;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic.Planetary.Factory;
using CodeBase.Logic.Planetary.Object;
using CodeBase.Logic.Planetary.System;

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
      var planetStaticData = assetsProvider.Load<PlanetStaticData>(AssetsPath.PLANET_STATIC_DATA);
      var planeteryObject = assetsProvider.Load<PlaneteryObject>(AssetsPath.PLANET);
      var planeterySystemBehaviour = assetsProvider.Load<PlaneterySystemBehaviour>(AssetsPath.PLANETARY_SYSTEM);
      _services.RegisterSingle<IPlaneterySystemFactory>(new PlaneterySystemFactory(planeterySystemBehaviour, planeteryObject, planetStaticData,
        _services.Single<IRandomService>()));
    }
  }
}