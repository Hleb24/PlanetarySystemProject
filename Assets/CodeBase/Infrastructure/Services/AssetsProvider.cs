using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services {
  public class AssetsProvider : IAssetsProvider {
    public T Load<T>(string path) where T : Object {
      return Resources.Load<T>(path);
    }

    public async ValueTask<T> LoadAsync<T>(string path) where T : Object {
      ResourceRequest resource = Resources.LoadAsync<T>(path);
      while (!resource.isDone) {
        await Task.Yield();
      }

      return resource.asset as T;
    }
  }
}