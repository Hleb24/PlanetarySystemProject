using System.Collections.Generic;
using CodeBase.Logic.Planetary;
using UnityEngine;

namespace CodeBase.Data.StaticData {
  [CreateAssetMenu(fileName = "PlanetsStaticData", menuName = "Static Data/Planets Data", order = 0)]
  public class PlanetsStaticData : ScriptableObject, IPlanetsData {
    [SerializeField]
    private List<PlanetData> _planets;
    [SerializeField]
    private float _stepForOrbitRadius;
    [SerializeField]
    private float _startOrbitRadius;

    public PlanetData this[MassClassEnum massClass] {
      get {
        return _planets.Find(data => data.MassClass == massClass);
      }
    }

    public PlanetData this[double mass] {
      get {
        if (mass > _planets.Find(data => data.MassClass == MassClassEnum.Jovian).MaxMass) {
          return this[MassClassEnum.Jovian];
        }

        if (mass < _planets.Find(data => data.MassClass == MassClassEnum.Asteroidan).MinMass) {
          return this[MassClassEnum.Asteroidan];
        }

        return _planets.Find(data => data.Is(mass));
      }
    }

    public float StepForOrbitRadius {
      get {
        return _stepForOrbitRadius;
      }
    }

    public float StartOrbitRadius {
      get {
        return _startOrbitRadius;
      }
    }
  }
}