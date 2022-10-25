using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services {
  public interface IAssetsProvider : IService {
    T Load<T>(string path) where T : Object;
    ValueTask<T> LoadAsync<T>(string path) where T : Object;
  }
}