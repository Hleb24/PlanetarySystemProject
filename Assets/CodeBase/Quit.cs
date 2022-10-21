namespace CodeBase {
  public static class Quit {
    public static void Out() {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
  }
}