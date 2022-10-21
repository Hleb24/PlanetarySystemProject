using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Data.StaticData;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic.Orbit;
using CodeBase.Logic.Planetary.Object;
using CodeBase.Logic.Planetary.System;
using ToolBox.Pools;
using UnityEngine;
using Logger = CodeBase.Infrastructure.Logger;

namespace CodeBase.Logic.Planetary.Factory {
  public class PlaneterySystemFactory : IPlaneterySystemFactory {
    private static IPlaneterySystem ReturnPlaneterySystem(List<IPlaneteryObject> planeteryObjects, List<OrbitRenderer> orbits, PlaneterySystemBehaviour systemBehaviour) {
      IPlaneterySystem planeterySystem = new PlaneterySystem(planeteryObjects);
      systemBehaviour.Construct(planeterySystem, orbits);
      return planeterySystem;
    }

    private static bool IsTotalMassLessThanZero(double totalMass) {
      return totalMass < 0;
    }

    private const float FULL_CIRCLE = 360.0f;

    private readonly GameObject _planetPrefab;
    private readonly GameObject _systemPrefab;
    private readonly GameObject _orbit;
    private readonly PlaneterySystemCalculator _calculator;
    private readonly IPlanetsData _planetsData;
    private readonly IRandomService _randomService;
    private readonly List<IPlaneteryObject> _planeteryObjects = new();
    private readonly List<OrbitRenderer> _orbits = new();
    private float _orbitRadius;
    private int _iteration;

    public PlaneterySystemFactory(GameObject planet, GameObject system, GameObject orbit, IPlanetsData planetsData, IRandomService randomService,
      PlaneterySystemCalculator calculator) {
      _planetPrefab = planet;
      _systemPrefab = system;
      _orbit = orbit;
      _planetsData = planetsData;
      _randomService = randomService;
      _calculator = calculator;
    }

    public IPlaneterySystem Create(double totalMass) {
      if (IsTotalMassLessThanZero(totalMass)) {
        Logger.Warning("Total mass less than 0!");
        return default;
      }

      PrepareCreation();
      if (IsOneAsteroidan(totalMass)) {
        InstantiatePlanet(_planeteryObjects, totalMass);
        Logger.Log("Only one Asteroidan");
        return ReturnPlaneterySystem(_planeteryObjects, _orbits, PlaneterySystemBehaviour);
      }

      Calculate(totalMass);

      return ReturnPlaneterySystem(_planeteryObjects, _orbits, PlaneterySystemBehaviour);
    }

    private void Calculate(double totalMass) {
      foreach (double mass in _calculator.CalculateMass(totalMass)) {
        InstantiatePlanet(_planeteryObjects, mass);
      }
    }

    private void PrepareCreation() {
      _orbitRadius = _planetsData.StartOrbitRadius;
      _planeteryObjects.Clear();
      _orbits.Clear();
      PlaneterySystemBehaviour = _systemPrefab.gameObject.Reuse<PlaneterySystemBehaviour>();
    }

    private void InstantiatePlanet(List<IPlaneteryObject> planeteryObjects, double planetMass) {
      float angle = _randomService.Next(0.0f, FULL_CIRCLE);
      PlaneteryObject planeteryObject = InitPlanet(PlaneterySystemBehaviour, angle);

      Transform planetTransform = planeteryObject.transform;
      var orbitalMovement = new OrbitalMovement(PlaneterySystemBehaviour.transform, planetTransform, angle, _randomService);
      ConstructPlanet(planeteryObjects, planeteryObject, orbitalMovement, planetMass);
    }

    private void ConstructPlanet(List<IPlaneteryObject> planeteryObjects, PlaneteryObject planeteryObject, OrbitalMovement orbitalMovement, double planetMass) {
      PlanetData planetData = _planetsData[planetMass];
      float planetRadius = (float)(planetMass / planetData.MaxMass) * planetData.MaxRadius;
      float planetScale = planetRadius * 2;
      _orbitRadius += planetRadius;
      planeteryObject.Construct(orbitalMovement, planetData.MassClass, planetMass, planetScale, _orbitRadius);
      planeteryObjects.Add(planeteryObject);
      InitOrbit(_orbitRadius);

      SummarizeRadius(planetRadius);
    }

    private void InitOrbit(float radius) {
      var orbit = _orbit.gameObject.Reuse<OrbitRenderer>(PlaneterySystemBehaviour.transform);
      orbit.Radius = radius;
      _orbits.Add(orbit);
    }

    private PlaneteryObject InitPlanet(PlaneterySystemBehaviour systemBehaviour, float angle) {
      Vector3 position = GetStartPosition(systemBehaviour.transform.position, angle);
      return _planetPrefab.gameObject.Reuse<PlaneteryObject>(position, Quaternion.identity, systemBehaviour.transform);
    }

    private Vector3 GetStartPosition(Vector3 systemPosition, float angle) {
      float positionX = systemPosition.x + Mathf.Sin(angle) * _orbitRadius;
      float positionZ = systemPosition.z + Mathf.Cos(angle) * _orbitRadius;
      return new Vector3(positionX, 0, positionZ);
    }

    private bool IsOneAsteroidan(double totalMass) {
      return totalMass <= _planetsData[MassClassEnum.Asteroidan].MaxMass;
    }

    private void SummarizeRadius(float planetRadius) {
      _orbitRadius += _planetsData.StepForOrbitRadius + planetRadius;
    }

    public PlaneterySystemBehaviour PlaneterySystemBehaviour { get; private set; }
  }
}