using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure;
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

    private const float STEP_ORBIT_RADIUS = 2.0f;

    private readonly List<MassClassEnum> _massClasses = new(Enum.GetValues(typeof(MassClassEnum)).Length);
    private readonly int _numberOfMassClass = Enum.GetValues(typeof(MassClassEnum)).Length;

    private readonly PlaneteryObject _planetPrefab;
    private readonly IPlanetInfo _planetInfo;
    private readonly IRandomService _randomService;
    private readonly PlaneterySystemBehaviour _planeterySystemBehaviourPrefab;
    private float _orbitRadius = 4;
    private int _iteration;

    public PlaneterySystemFactory(PlaneterySystemBehaviour planeterySystemBehaviourPrefab, PlaneteryObject planetPrefab, IPlanetInfo planetInfo, IRandomService randomService) {
      _planeterySystemBehaviourPrefab = planeterySystemBehaviourPrefab;
      _planetPrefab = planetPrefab;
      _planetInfo = planetInfo;
      _randomService = randomService;
      SetupMassClasses();
    }

    public IPlaneterySystem Create(double totalMass) {
      if (totalMass < 0) {
        Logger.Warning("Total mass less than 0!");
        return default;
      }

      double leftMass = totalMass;
      PrepareCreation();
      var planeteryObjects = new List<IPlaneteryObject>();

      PlaneterySystemBehaviour = InstantiatePlaneterySystemBehaviour();

      int maxIteration = _randomService.Next(1, _numberOfMassClass);

      if (leftMass <= _planetInfo[MassClassEnum.Asteroidan].MaxMass) {
        InstantiatePlanet(PlaneterySystemBehaviour, leftMass, planeteryObjects);
        Logger.Log("Only one Asteroidan");

        return ReturnPlaneterySystem(planeteryObjects, PlaneterySystemBehaviour);
      }

      while (leftMass > 0) {
        PlanetData planet = _planetInfo[leftMass];
        MassClassEnum massClassEnum = _massClasses.Where((_, i) => i <= (int)planet.MassClass).ToList().Random();

        double minMass = _planetInfo[massClassEnum].MinMass;
        double maxMass = _planetInfo[massClassEnum].MaxMass;
        maxMass = maxMass >= leftMass ? leftMass : maxMass;

        double planetMass = _randomService.Next(minMass, maxMass);
        if (_iteration >= maxIteration) {
          planetMass = _planetInfo[leftMass].MaxMass >= leftMass ? leftMass : _planetInfo[leftMass].MaxMass;
          
          Logger.Log("Mass Class " + massClassEnum);
          InstantiatePlanet(PlaneterySystemBehaviour, planetMass, planeteryObjects);
          leftMass -= planetMass;

          _iteration++;
          continue;
        }

        if (planetMass > leftMass) {
          continue;
        }

        Logger.Log("Mass Class " + massClassEnum);
        InstantiatePlanet(PlaneterySystemBehaviour, planetMass, planeteryObjects);
        leftMass -= planetMass;

        _iteration++;
      }

      return ReturnPlaneterySystem(planeteryObjects, PlaneterySystemBehaviour);
    }

    private void PrepareCreation() {
      _iteration = 0;
      _orbitRadius = 4;
    }

    private void SetupMassClasses() {
      for (var i = 0; i < _numberOfMassClass; i++) {
        _massClasses.Add((MassClassEnum)i);
      }
    }

    private void InstantiatePlanet(PlaneterySystemBehaviour systemBehaviour, double planetMass, List<IPlaneteryObject> planeteryObjects) {
      PlanetData planetData = _planetInfo[planetMass];

      float planetRadius = (float)(planetMass / planetData.MaxMass) * planetData.MaxRadius;
      float planetScale = planetRadius * 2;
      _orbitRadius += planetRadius;

      float angle = _randomService.Next(0.0f, 360.0f);
      Vector3 position = GetStartPosition(systemBehaviour, angle);

      PlaneteryObject planeteryObject = UnityEngine.Object.Instantiate(_planetPrefab, position, Quaternion.identity, systemBehaviour.transform);

      Transform planetTransform = planeteryObject.transform;
      var orbitalMovement = new OrbitalMovement(systemBehaviour.transform, planetTransform, angle, _randomService);
      planeteryObject.Construct(orbitalMovement, planetData.MassClass, planetMass, planetScale, _orbitRadius);

      planeteryObjects.Add(planeteryObject);
      _orbitRadius += STEP_ORBIT_RADIUS + planetRadius;
    }

    private Vector3 GetStartPosition(PlaneterySystemBehaviour systemBehaviour, float angle) {
      float positionX = systemBehaviour.transform.position.x + Mathf.Sin(angle) * _orbitRadius;
      float positionZ = systemBehaviour.transform.position.z + Mathf.Cos(angle) * _orbitRadius;

      var position = new Vector3(positionX, 0, positionZ);
      return position;
    }

    private PlaneterySystemBehaviour InstantiatePlaneterySystemBehaviour() {
      return UnityEngine.Object.Instantiate(_planeterySystemBehaviourPrefab);
    }

    public PlaneterySystemBehaviour PlaneterySystemBehaviour { get; private set; }
  }
}