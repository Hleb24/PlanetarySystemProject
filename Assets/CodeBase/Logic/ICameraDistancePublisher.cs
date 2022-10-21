using System;

namespace CodeBase.Logic {
  public interface ICameraDistancePublisher {
    event Action<float> CameraDistanceChanged;
  }
}