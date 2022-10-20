using CodeBase.Logic.Planetary;

namespace CodeBase.Infrastructure.Services {
  public interface IRandomService : IService {
    void InitState();
    int Next(int min, int max);
    float Next(float min, float max);
    float Next(double min, double max);
  }
}