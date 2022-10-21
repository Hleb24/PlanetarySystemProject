using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure;
using CodeBase.Logic.Planetary.Object;

namespace CodeBase.Logic.Planetary {
  public static class PlaneModelCalculator {
    public static IEnumerable<PlanetModel> UpdatePlanetInfo(IEnumerable<IPlaneteryObject> planeteryObjects) {
      IEnumerable<IGrouping<MassClassEnum, IPlaneteryObject>> groupBy = planeteryObjects.GroupBy(p => p.MassClass);
      var existMassClass = new List<MassClassEnum>();
      foreach (IGrouping<MassClassEnum, IPlaneteryObject> planetery in groupBy) {
        existMassClass.Add(planetery.Key);
        var count = 0;
        foreach (IPlaneteryObject _ in planetery) {
          count++;
        }

        yield return new PlanetModel((int)planetery.Key, planetery.Key.ToString(), count);
      }

      int length = Constants.NumberOfMassClass;
      for (var i = 0; i < length; i++) {
        if (existMassClass.Contains((MassClassEnum)i)) {
          continue;
        }

        yield return new PlanetModel(i, ((MassClassEnum)i).ToString(), 0);
      }
    }
  }
}