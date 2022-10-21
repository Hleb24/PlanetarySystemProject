using CodeBase.Data.StaticData;
using CodeBase.Logic.Planetary.System;
using ToolBox.Pools;

namespace CodeBase.Logic.Planetary.Factory {
  public class PlaneterySystemRebuilder {
    private readonly IPlaneterySystemData _planeteryStaticData;
    private readonly PlaneterySystemFactory _planeterySystemFactory;

    public PlaneterySystemRebuilder(IPlaneterySystemData planeteryStaticData, PlaneterySystemFactory planeterySystemFactory) {
      _planeteryStaticData = planeteryStaticData;
      _planeterySystemFactory = planeterySystemFactory;
    }

    public IPlaneterySystem Recreate() {
      // UnityEngine.Object.Destroy(_planeterySystemFactory?.PlaneterySystemBehaviour.gameObject);
      _planeterySystemFactory?.PlaneterySystemBehaviour.gameObject.Release();
      return _planeterySystemFactory?.Create(_planeteryStaticData.TotalMass);
    }
  }
}