namespace CodeBase.Infrastructure.States {
  public interface IPayLoadedState<TPayload> : IExitableState {
    void Enter(TPayload payLoad);
  }
}