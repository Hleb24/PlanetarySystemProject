using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace CodeBase.Infrastructure {
  public class Logger {
    [Conditional("UNITY_ASSERTIONS")]
    public static void Log(string message) {
      Debug.Log(message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void Warning(string message) {
      Debug.LogWarning(message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void Error(string message) {
      Debug.LogError(message);
    }
  }
}