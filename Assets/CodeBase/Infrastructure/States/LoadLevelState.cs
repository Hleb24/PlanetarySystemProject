using CodeBase.Data.StaticData;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic.Planetary.Factory;
using CodeBase.Logic.Planetary.System;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.States {
  public class LoadLevelState : IState {
    private readonly IAssetsProvider _assetsProvider;
    private readonly IPlaneterySystemFactory _planeterySystemFactory;
    private IPlaneterySystem _planeterySystem;

    public LoadLevelState(IAssetsProvider assetsProvider, IPlaneterySystemFactory planeterySystemFactory) {
      _assetsProvider = assetsProvider;
      _planeterySystemFactory = planeterySystemFactory;
    }

    public void Enter() {
      InitPlaneterySystem();
    }

    public void Exit() { }

    private void InitPlaneterySystem() {
      var planeterySystemStaticData = _assetsProvider.Load<PlaneterySystemStaticData>(AssetsPath.PLANETERY_SYSTEM_STATIC_DATA);
      _planeterySystem = _planeterySystemFactory.Create(planeterySystemStaticData.TotalMass);
      if (_planeterySystemFactory is PlaneterySystemFactory factory) {
        InitUI(factory, _planeterySystem, planeterySystemStaticData);
      }
    }

    private void InitUI(PlaneterySystemFactory planeterySystemFactory, IPlaneterySystem systemStaticData, PlaneterySystemStaticData planeterySystemStaticData) {
      var hudPrefab = _assetsProvider.Load<GameObject>(AssetsPath.HUD);
      var reloadContainerPrefab = _assetsProvider.Load<ReloadContainer>(AssetsPath.RELOAD_CONTAINER);
      GameObject hud = Instantiate(hudPrefab);
      ReloadContainer reloadContainer = Instantiate(reloadContainerPrefab, hud.transform);
      reloadContainer.Construct(planeterySystemFactory, systemStaticData, planeterySystemStaticData);
    }

    private T Instantiate<T>(T prefab) where T : Object {
      return Object.Instantiate(prefab);
    }

    private T Instantiate<T>(T prefab, Transform parent) where T : Object {
      return Object.Instantiate(prefab, parent);
    }
  }
}