using System.Collections.Generic;
using CodeBase.Logic.Planetary.Object;

namespace CodeBase.Logic.Planetary.System {
  public interface IPlaneterySystem {
    void Update(float deltaTime);
    IEnumerable<IPlaneteryObject> PlaneteryObjects { get; }
  }
}