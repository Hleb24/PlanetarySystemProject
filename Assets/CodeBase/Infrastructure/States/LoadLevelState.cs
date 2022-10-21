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
      var mediator = new UpdatePlaneterySystemMediator(planeterySystemUIContainer, rebuilder, planetUI.SetInfo);
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

    public void Enter() {
      InitPlaneterySystem();
    }

    public void Exit() { }

    private void InitPlaneterySystem() {
      var planeterySystemStaticData = _assetsProvider.Load<PlaneterySystemStaticData>(AssetsPath.PLANETERY_SYSTEM_STATIC_DATA);
      IPlaneterySystem planeterySystem = _planeterySystemFactory.Create(planeterySystemStaticData.TotalMass);
      if (_planeterySystemFactory is not PlaneterySystemFactory factory) {
        Logger.Error($"{nameof(PlaneterySystemFactory)} not implemented {nameof(IPlaneterySystemFactory)!}");
        return;
      }

      var rebuilder = new PlaneterySystemRebuilder(planeterySystemStaticData, factory);
      InstantiateUI(planeterySystemStaticData, rebuilder, planeterySystem);
    }

    private void InstantiateUI(PlaneterySystemStaticData planeterySystemStaticData, PlaneterySystemRebuilder rebuilder, IPlaneterySystem planeterySystem) {
      GameObject hud = InitHUD();
      PlanetUI planetUI = InitPlanetUI(hud.transform);
      PlaneterySystemUIContainer planeterySystemUIContainer = InitUpdatePlaneterySystem(hud.transform);
      PrepareUpdatePlaneterySystem(planeterySystemStaticData, planeterySystemUIContainer);
      IniMediator(rebuilder, planeterySystem, planeterySystemUIContainer, planetUI);
    }

    private GameObject InitHUD() {
      var hud = _assetsProvider.Load<GameObject>(AssetsPath.HUD);
      return Instantiate(hud);
    }

    private PlanetUI InitPlanetUI(Transform hud) {
      var planetPrefab = _assetsProvider.Load<PlanetUI>(AssetsPath.PLANET_UI);
      PlanetUI planetaryUI = Object.Instantiate(planetPrefab, hud);
      planetaryUI.Quit += AppQuitHelper.Quit;
      return planetaryUI;
    }

    private PlaneterySystemUIContainer InitUpdatePlaneterySystem(Transform hud) {
      var updatePlaneterySystemContainer = _assetsProvider.Load<PlaneterySystemUIContainer>(AssetsPath.PLANETERY_SYSTEM_UI_CONTAINER);
      return Instantiate(updatePlaneterySystemContainer, hud);
    }
  }
}