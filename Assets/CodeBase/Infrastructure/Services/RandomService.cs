using System;
using Random = UnityEngine.Random;

namespace CodeBase.Infrastructure.Services {
  public class RandomService : IRandomService {
    public void InitState() {
      Random.InitState((int)DateTime.Now.Ticks);
    }

    public int Next(int min, int max) {
      return Random.Range(min, max);
    }

    public float Next(float min, float max) {
      return Random.Range(min, max);
    }

    public float Next(double min, double max) {
      return Random.Range(Convert.ToSingle(min), Convert.ToSingle(max));
    }
  }
}