using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic.Orbit;
using CodeBase.Logic.Planetary.Object;
using ToolBox.Pools;
using UnityEngine;

namespace CodeBase.Logic.Planetary.System {
  public class PlaneterySystemBehaviour : MonoBehaviour, IPoolable {
    private IPlaneterySystem _planeterySystem;
    private IEnumerable<OrbitRenderer> _orbitRenderers;

    private void Update() {
      _planeterySystem?.Update(Time.deltaTime);
    }

    public void OnReuse() { }

    public void OnRelease() {
      foreach (PlaneteryObject planet in _planeterySystem.PlaneteryObjects.Cast<PlaneteryObject>()) {
        planet.gameObject.Release();
      }

      foreach (OrbitRenderer orbit in _orbitRenderers) {
        orbit.gameObject.Release();
      }
    }

    public void Construct(IPlaneterySystem planeterySystem, List<OrbitRenderer> orbits) {
      _planeterySystem = planeterySystem;
      _orbitRenderers = orbits;
    }
  }
}