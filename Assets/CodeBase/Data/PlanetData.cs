using System;
using CodeBase.Logic.Planetary;

namespace CodeBase.Data {
  [Serializable]
  public class PlanetData {
    public MassClassEnum MassClass;
    public double MinMass;
    public double MaxMass;
    public float MinRadius;
    public float MaxRadius;

    public bool Is(double mass) {
      return mass >= MinMass && mass <= MaxMass;
    }
  }
}