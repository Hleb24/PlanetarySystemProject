using UnityEngine;

namespace CodeBase.Logic.Planetary.System {
  public class PlaneterySystemBehaviour : MonoBehaviour {
    private IPlaneterySystem _planeterySystem;

    private void Update() {
      _planeterySystem?.Update(Time.deltaTime);
    }

    public void Construct(IPlaneterySystem planeterySystem) {
      _planeterySystem = planeterySystem;
    }
  }
}