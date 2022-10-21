using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic.Planetary.Factory;

namespace CodeBase.Infrastructure.States {
  public class GameStateMachine : IGameStateMachine {
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(AllServices services) {
      _states = new Dictionary<Type, IExitableState> {
        { typeof(BootstrapState), new BootstrapState(this, services) },
        { typeof(LoadLevelState), new LoadLevelState(services.Single<IPlaneterySystemFactory>(), services.Single<IAssetsProvider>()) }
      };
    }

    public void Enter<TState>() where TState : class, IState {
      IState state = ChangeState<TState>();
      state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payLoad) where TState : class, IPayLoadedState<TPayload> {
      var state = ChangeState<TState>();
      state.Enter(payLoad);
    }

    private TState ChangeState<TState>() where TState : class, IExitableState {
      _activeState?.Exit();
      var state = GetState<TState>();
      _activeState = state;
      return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState {
      return _states[typeof(TState)] as TState;
    }
  }
}