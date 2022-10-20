using CodeBase.Infrastructure.Services;
using CodeBase.Logic.Planetary.System;

namespace CodeBase.Logic.Planetary.Factory {
  public interface IPlaneterySystemFactory : IService {
    IPlaneterySystem Create(double totalMass);
  }
}