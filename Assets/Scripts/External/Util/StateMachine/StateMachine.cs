using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GDEUtils.StateMachine
{
  public class StateMachine<T> {
    public Stack<State<T>> StateStack { get; private set; }

    public State<T> CurrentState { get => StateStack.Count > 0 ? StateStack.Peek() : null; }

    public State<T> PrevState { get => GetPrevState(); }

    public T Owner;

    public StateMachine(T owner) {
      this.Owner = owner;
      StateStack = new Stack<State<T>>();
    }

    public StateMachine(T owner, State<T> startState) {
      this.Owner = owner;
      StateStack = new Stack<State<T>>();
      Push(startState);
    }

    public virtual void Destroy() {
      PopAll();
    }

    public void Execute() {
      CurrentState?.Update();
    }

    public void Push(State<T> newState) {
      newState.Enter();
      StateStack.Push(newState);
    }

    public IEnumerator PushAndWaitUntilReturn(State<T> newState) {
      var oldState = CurrentState;
      Push(newState);
      yield return new WaitUntil(() => CurrentState == oldState);
    }

    public State<T> Pop() {
      CurrentState.Exit();
      var removedState = StateStack.Pop();
      return removedState;
    }

    public void PopAndPush(State<T> newState) {
      if (CurrentState != null) {
        Pop();
      }

      Push(newState);
    }

    public void PopAll() {
      while (CurrentState != null) {
        Pop();
      }
    }

    public void PopAllAndPush(State<T> newState) {
      PopAll();

      Push(newState);
    }

    public State<T> GetPrevState() {
      return StateStack.ElementAt(1);
    }
  }
}
