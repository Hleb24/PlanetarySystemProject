using System;

namespace CodeBase.Logic {
  public interface IReloadPublisher {
    event Action Reload;
  }
}