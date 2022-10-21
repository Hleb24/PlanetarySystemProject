using System;

namespace CodeBase.Infrastructure {
  public interface IQuitable {
    event Action<IQuitable> Quit;
  }
}