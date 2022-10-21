#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CodeBase.Infrastructure {
  public static class AppQuitHelper {
    public static void Quit(IQuitable quitable) {
      if (quitable is null) {
        return;
      }

#if UNITY_EDITOR
      EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
  }
}