using CodeBase.Logic.Orbit;
using ToolBox.Pools;
using UnityEngine;

namespace CodeBase.Logic.Planetary.Object {
  public class PlaneteryObject : MonoBehaviour, IPlaneteryObject, IPoolable {
    [SerializeField]
    private MassClassEnum _massClass;
    [SerializeField]
    private double _mass;
    [SerializeField]
    private Transform _planet;
    private OrbitalMovement _orbitalMovement;

    public void OnReuse() { }

    public void OnRelease() { }

    public void Construct(OrbitalMovement orbitalMovement, MassClassEnum massClass, double mass, float planetScale, float orbitRadius) {
      _orbitalMovement = orbitalMovement;
      MassClass = massClass;
      Mass = mass;
      _planet.localScale = new Vector3(planetScale, planetScale, planetScale);
      _orbitalMovement.Radius = orbitRadius;
    }

    public void MoveByOrbit(float deltaTime) {
      _orbitalMovement.MoveByOrbit(deltaTime);
    }

    public MassClassEnum MassClass {
      get {
        return _massClass;
      }
      private set {
        _massClass = value;
      }
    }

    public double Mass {
      get {
        return _mass;
      }
      private set {
        _mass = value;
      }
    }
  }
}