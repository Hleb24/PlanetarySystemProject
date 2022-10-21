using System;
using CodeBase.Logic.Planetary;

namespace CodeBase.Infrastructure {
  public static class Constants {
    public static readonly int NumberOfMassClass = Enum.GetValues(typeof(MassClassEnum)).Length;
  }
}