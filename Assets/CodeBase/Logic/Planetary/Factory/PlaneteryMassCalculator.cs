using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Data.StaticData;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Logic.Planetary.Factory {
  public class PlaneterySystemCalculator {
    private readonly List<MassClassEnum> _massClasses = new(Constants.NumberOfMassClass);

    private readonly IPlanetsData _planetsData;
    private readonly IRandomService _randomService;
    private int _iteration;

    public PlaneterySystemCalculator(IPlanetsData planetsData, IRandomService randomService) {
      _planetsData = planetsData;
      _randomService = randomService;
      SetupMassClasses();
    }

    public IEnumerable<double> CalculateMass(double totalMass) {
      double leftMass = totalMass;
      _iteration = 0;

      int maxIteration = _randomService.Next(1, Constants.NumberOfMassClass);

      while (leftMass > 0) {
        PlanetData planet = _planetsData[leftMass];
        MassClassEnum massClassEnum = _massClasses.Where((_, i) => i <= (int)planet.MassClass).ToList().Random();

        double minMass = _planetsData[massClassEnum].MinMass;
        double maxMass = _planetsData[massClassEnum].MaxMass;
        maxMass = maxMass >= leftMass ? leftMass : maxMass;

        double planetMass = _randomService.Next(minMass, maxMass);
        if (_iteration >= maxIteration) {
          planetMass = _planetsData[leftMass].MaxMass >= leftMass ? leftMass : _planetsData[leftMass].MaxMass;
          yield return planetMass;

          leftMass -= planetMass;
          _iteration++;
          continue;
        }

        if (planetMass > leftMass) {
          continue;
        }

        yield return planetMass;
        leftMass -= planetMass;

        _iteration++;
      }
    }

    private void SetupMassClasses() {
      for (var i = 0; i < Constants.NumberOfMassClass; i++) {
        _massClasses.Add((MassClassEnum)i);
      }
    }
  }
}