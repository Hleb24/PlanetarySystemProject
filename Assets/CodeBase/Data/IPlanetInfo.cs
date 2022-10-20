using CodeBase.Logic.Planetary;

namespace CodeBase.Data {
  public interface IPlanetInfo {
    PlanetData this[MassClassEnum massClass] { get; }
    PlanetData this[double mass] { get; }
  }
}