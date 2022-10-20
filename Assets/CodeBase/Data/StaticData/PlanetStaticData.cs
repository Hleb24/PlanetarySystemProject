using System.Collections.Generic;
using CodeBase.Logic.Planetary;
using UnityEngine;

namespace CodeBase.Data.StaticData {
  [CreateAssetMenu(fileName = "PlanetStaticData", menuName = "Static Data/Planet Data", order = 0)]
  public class PlanetStaticData : ScriptableObject, IPlanetInfo {
    [SerializeField]
    private List<PlanetData> _planets;

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
  }
}