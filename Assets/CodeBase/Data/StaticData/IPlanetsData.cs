using CodeBase.Logic.Planetary;

namespace CodeBase.Data.StaticData {
  public interface IPlanetsData {
    PlanetData this[MassClassEnum massClass] { get; }
    PlanetData this[double mass] { get; }
    float StepForOrbitRadius { get; }
    float StartOrbitRadius { get; }
  }
}