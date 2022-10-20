using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure {
  public class Game {
    public readonly IGameStateMachine StateMachine;

    public Game() {
      StateMachine = new GameStateMachine(AllServices.Container);
    }
  }
}