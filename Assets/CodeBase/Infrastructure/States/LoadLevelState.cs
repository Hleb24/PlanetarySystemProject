using System.Threading.Tasks;
using CodeBase.Data.StaticData;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic.Cameras;
using CodeBase.Logic.Planetary.Factory;
using CodeBase.Logic.Planetary.System;
using CodeBase.UI;
using CodeBase.UI.Mediators;
using UnityEngine;

namespace CodeBase.Infrastructure.States {
  public class LoadLevelState : IState {
    private static T Instantiate<T>(T prefab) where T : Object {
      return Object.Instantiate(prefab);
    }

    private static T Instantiate<T>(T prefab, Transform parent) where T : Object {
      return Object.Instantiate(prefab, parent);
    }

    private static void IniMediator(PlaneterySystemRebuilder rebuilder, IPlaneterySystem planeterySystem, PlaneterySystemUIContainer planeterySystemUIContainer,
      PlanetUI planetUI) {
      var mediator = new PlaneterySystemUIMediator(planeterySystemUIContainer, rebuilder, planetUI.SetInfo);
      mediator.UpdateInfo(planeterySystem);
    }

    private static void InitCameraZoom(PlaneterySystemUIContainer planeterySystemUIContainer) {
      if (Camera.main != null) {
        var _ = new CameraZoom(Camera.main.transform, planeterySystemUIContainer);
      }
    }

    private static void PrepareUpdatePlaneterySystem(PlaneterySystemStaticData planeterySystemStaticData, PlaneterySystemUIContainer planeterySystemUIContainer) {
      InitCameraZoom(planeterySystemUIContainer);
      planeterySystemUIContainer.Construct((float)planeterySystemStaticData.TotalMass);
      planeterySystemUIContainer.TotalMassChanged += newMass => planeterySystemStaticData.TotalMass = newMass;
    }

    private readonly IPlaneterySystemFactory _planeterySystemFactory;
    private readonly IAssetsProvider _assetsProvider;

    public LoadLevelState(IPlaneterySystemFactory planeterySystemFactory, IAssetsProvider assetsProvider) {
      _planeterySystemFactory = planeterySystemFactory;
      _assetsProvider = assetsProvider;
    }

    public async void Enter() {
      await InitPlaneterySystem();
    }

    public void Exit() { }

    private async ValueTask InitPlaneterySystem() {
      await InitObjectPool();
      var planeterySystemStaticData = await _assetsProvider.LoadAsync<PlaneterySystemStaticData>(AssetsPath.PLANETERY_SYSTEM_STATIC_DATA);
      IPlaneterySystem planeterySystem = _planeterySystemFactory.Create(planeterySystemStaticData.TotalMass);
      if (_planeterySystemFactory is not PlaneterySystemFactory factory) {
        Logger.Error($"{nameof(PlaneterySystemFactory)} not implemented {nameof(IPlaneterySystemFactory)!}");
        return;
      }

      var rebuilder = new PlaneterySystemRebuilder(planeterySystemStaticData, factory);
      await InstantiateUI(planeterySystemStaticData, rebuilder, planeterySystem);
    }

    private async ValueTask InitObjectPool() {
      var objectPool = await _assetsProvider.LoadAsync<GameObject>(AssetsPath.OBJECT_POOL);
      Instantiate(objectPool);
    }

    private async ValueTask InstantiateUI(PlaneterySystemStaticData planeterySystemStaticData, PlaneterySystemRebuilder rebuilder, IPlaneterySystem planeterySystem) {
      GameObject hud = await InitHUD();
      PlanetUI planetUI = await InitPlanetUI(hud.transform);
      PlaneterySystemUIContainer planeterySystemUIContainer = await InitUpdatePlaneterySystem(hud.transform);
      PrepareUpdatePlaneterySystem(planeterySystemStaticData, planeterySystemUIContainer);
      IniMediator(rebuilder, planeterySystem, planeterySystemUIContainer, planetUI);
    }

    private async ValueTask<GameObject> InitHUD() {
      var hud = await _assetsProvider.LoadAsync<GameObject>(AssetsPath.HUD);
      return Instantiate(hud);
    }

    private async ValueTask<PlanetUI> InitPlanetUI(Transform hud) {
      var planetPrefab = await _assetsProvider.LoadAsync<PlanetUI>(AssetsPath.PLANET_UI);
      PlanetUI planetaryUI = Object.Instantiate(planetPrefab, hud);
      planetaryUI.Quit += AppQuitHelper.Quit;
      return planetaryUI;
    }

    private async ValueTask<PlaneterySystemUIContainer> InitUpdatePlaneterySystem(Transform hud) {
      var updatePlaneterySystemContainer = await _assetsProvider.LoadAsync<PlaneterySystemUIContainer>(AssetsPath.PLANETERY_SYSTEM_UI_CONTAINER);
      return Instantiate(updatePlaneterySystemContainer, hud);
    }
  }
}