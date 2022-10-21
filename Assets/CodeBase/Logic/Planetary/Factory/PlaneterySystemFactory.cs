using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic.Orbit;
using CodeBase.Logic.Planetary.Object;
using CodeBase.Logic.Planetary.System;
using UnityEngine;
using Logger = CodeBase.Infrastructure.Logger;

namespace CodeBase.Logic.Planetary.Factory {
  public class PlaneterySystemFactory : IPlaneterySystemFactory {
    private static IPlaneterySystem ReturnPlaneterySystem(List<IPlaneteryObject> planeteryObjects, PlaneterySystemBehaviour systemBehaviour) {
      IPlaneterySystem planeterySystem = new PlaneterySystem(planeteryObjects);
      systemBehaviour.Construct(planeterySystem);
      return planeterySystem;
    }

    private static bool IsTotalMassLessThanZero(double totalMass) {
      return totalMass < 0;
    }

    private const float FULL_CIRCLE = 360.0f;

    private readonly PlaneteryObject _planetPrefab;
    private readonly PlaneterySystemBehaviour _systemPrefab;
    private readonly PlaneterySystemCalculator _calculator;
    private readonly IPlanetsData _planetsData;
    private readonly IRandomService _randomService;
    private readonly List<IPlaneteryObject> _planeteryObjects = new();
    private float _orbitRadius;
    private int _iteration;

    public PlaneterySystemFactory(PlaneteryObject planet, PlaneterySystemBehaviour system, IPlanetsData planetsData, IRandomService randomService,
      PlaneterySystemCalculator calculator) {
      _planetPrefab = planet;
      _systemPrefab = system;
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
        InstantiatePlanet(_planeteryObjects, PlaneterySystemBehaviour, totalMass);
        Logger.Log("Only one Asteroidan");
        return ReturnPlaneterySystem(_planeteryObjects, PlaneterySystemBehaviour);
      }

      Calculate(totalMass);

      return ReturnPlaneterySystem(_planeteryObjects, PlaneterySystemBehaviour);
    }

    private void Calculate(double totalMass) {
      foreach (double mass in _calculator.CalculateMass(totalMass)) {
        InstantiatePlanet(_planeteryObjects, PlaneterySystemBehaviour, mass);
      }
    }

    private void PrepareCreation() {
      _orbitRadius = _planetsData.StartOrbitRadius;
      _planeteryObjects.Clear();
      PlaneterySystemBehaviour = UnityEngine.Object.Instantiate(_systemPrefab);
    }

    private void InstantiatePlanet(List<IPlaneteryObject> planeteryObjects, PlaneterySystemBehaviour systemBehaviour, double planetMass) {
      float angle = _randomService.Next(0.0f, FULL_CIRCLE);
      PlaneteryObject planeteryObject = InitPlanet(systemBehaviour, angle);
      Transform planetTransform = planeteryObject.transform;
      var orbitalMovement = new OrbitalMovement(systemBehaviour.transform, planetTransform, angle, _randomService);
      ConstructPlanet(planeteryObjects, planeteryObject, orbitalMovement, planetMass);
    }

    private void ConstructPlanet(List<IPlaneteryObject> planeteryObjects, PlaneteryObject planeteryObject, OrbitalMovement orbitalMovement, double planetMass) {
      PlanetData planetData = _planetsData[planetMass];
      float planetRadius = (float)(planetMass / planetData.MaxMass) * planetData.MaxRadius;
      float planetScale = planetRadius * 2;
      _orbitRadius += planetRadius;
      planeteryObject.Construct(orbitalMovement, planetData.MassClass, planetMass, planetScale, _orbitRadius);
      planeteryObjects.Add(planeteryObject);
      SummarizeRadius(planetRadius);
    }

    private PlaneteryObject InitPlanet(PlaneterySystemBehaviour systemBehaviour, float angle) {
      Vector3 position = GetStartPosition(systemBehaviour.transform.position, angle);
      return UnityEngine.Object.Instantiate(_planetPrefab, position, Quaternion.identity, systemBehaviour.transform);
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