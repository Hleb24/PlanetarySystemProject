using System;

namespace CodeBase.Logic.Planetary {
  public interface IReloadPublisher {
    event Action Reload;
  }
}