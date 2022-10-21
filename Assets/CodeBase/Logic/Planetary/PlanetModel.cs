namespace CodeBase.Logic.Planetary {
  public class PlanetModel {
    private readonly int _massClass;
    private readonly string _name;
    private readonly int _numberOfPlanet;

    public PlanetModel(int massClass, string name, int numberOfPlanet) {
      _massClass = massClass;
      _name = name;
      _numberOfPlanet = numberOfPlanet;
    }

    public void Deconstruct(out int massClass, out string name, out int numberOfPlanet) {
      massClass = _massClass;
      name = _name;
      numberOfPlanet = _numberOfPlanet;
    }
  }
}