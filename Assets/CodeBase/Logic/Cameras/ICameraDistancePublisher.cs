using System;

namespace CodeBase.Logic.Cameras {
  public interface ICameraDistancePublisher {
    event Action<float> CameraDistanceChanged;
  }
}