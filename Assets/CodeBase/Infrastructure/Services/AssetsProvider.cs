using UnityEngine;

namespace CodeBase.Infrastructure.Services {
  public class AssetsProvider : IAssetsProvider {
    public T Load<T>(string path) where T : Object {
      return Resources.Load<T>(path);
    }
  }
}