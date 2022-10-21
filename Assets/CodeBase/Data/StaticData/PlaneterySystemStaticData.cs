using System.Globalization;
using UnityEngine;

namespace CodeBase.Data.StaticData {
  [CreateAssetMenu(fileName = "PlaneterySystemStaticData", menuName = "Static Data/Planetery System Static Data", order = 0)]
  public class PlaneterySystemStaticData : ScriptableObject, IPlaneterySystemData {
    [SerializeField]
    private double _totalMass = 10;

    public override string ToString() {
      return _totalMass.ToString(CultureInfo.CurrentCulture);
    }

    public double TotalMass {
      get {
        return _totalMass;
      }
      set {
        _totalMass = value;
      }
    }
  }
}