namespace CodeBase.Infrastructure {
  public static class AppQuitHelper {
    public static void Quit(IQuitable quitable) {
      if (quitable is null) {
        return;
      }

#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      UnityEngine.Application.Quit();
#endif
    }
  }
}