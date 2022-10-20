using CodeBase.Logic.Orbit;
using UnityEngine;

namespace CodeBase.Logic.Planetary.Object {
  public class PlaneteryObject : MonoBehaviour, IPlaneteryObject {
    [SerializeField]
    private MassClassEnum _massClass;
    [SerializeField]
    private double _mass;
    [SerializeField]
    private Transform _planet;
    [SerializeField]
    private OrbitRenderer _orbitRenderer;
    private OrbitalMovement _orbitalMovement;

    public void Construct(OrbitalMovement orbitalMovement, MassClassEnum massClass, double mass, float planetScale, float orbitRadius) {
      _orbitalMovement = orbitalMovement;
      MassClass = massClass;
      Mass = mass;
      _planet.localScale = new Vector3(planetScale, planetScale, planetScale);
      _orbitalMovement.Radius = orbitRadius;
      _orbitRenderer.Radius = orbitRadius;
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