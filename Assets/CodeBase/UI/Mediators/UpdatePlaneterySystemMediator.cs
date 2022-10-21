using System;
using CodeBase.Logic;
using CodeBase.Logic.Planetary;
using CodeBase.Logic.Planetary.Factory;
using CodeBase.Logic.Planetary.System;

namespace CodeBase.UI.Mediators {
  public class UpdatePlaneterySystemMediator {
    private readonly PlaneterySystemRebuilder _rebuilder;
    private readonly Action<int, string, int> _setInfoAction;

    public UpdatePlaneterySystemMediator(IReloadPublisher cameraDistance, PlaneterySystemRebuilder rebuilder, Action<int, string, int> setInfoAction) {
      cameraDistance.Reload += OnReload;
      _rebuilder = rebuilder;
      _setInfoAction = setInfoAction;
    }

    public void UpdateInfo(IPlaneterySystem planetarySystem) {
      SetInfo(planetarySystem);
    }

    private void OnReload() {
      IPlaneterySystem planetarySystem = _rebuilder.Recreate();
      SetInfo(planetarySystem);
    }

    private void SetInfo(IPlaneterySystem planetarySystem) {
      foreach (PlanetModel planetModel in PlaneModelCalculator.UpdatePlanetInfo(planetarySystem?.PlaneteryObjects)) {
        var (index, name, number) = planetModel;
        _setInfoAction.Invoke(index, name, number);
      }
    }
  }
}