using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic.Planetary.Object;

namespace CodeBase.Logic.Planetary.System {
  public class PlaneterySystem : IPlaneterySystem {
    private readonly IEnumerable<PlaneteryObject> _planets;

    public PlaneterySystem(IEnumerable<IPlaneteryObject> planeteryObjects) {
      PlaneteryObjects = planeteryObjects;
      _planets = PlaneteryObjects.Cast<PlaneteryObject>();
    }

    public void Update(float deltaTime) {
      foreach (PlaneteryObject planet in _planets) {
        planet.MoveByOrbit(deltaTime);
      }
    }

    public IEnumerable<IPlaneteryObject> PlaneteryObjects { get; }
  }
}