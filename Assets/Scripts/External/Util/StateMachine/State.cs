using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDEUtils.StateMachine {
  public class State<T> {
    public T Owner { get; private set; }
    public StateMachine<T> StateMachine { get; private set; }

    private string animationEnterName;
    

    protected float stateTimer;

    protected bool triggerCalled;

    public State(T owner, StateMachine<T> stateMachine, string animationEnterName) {
      this.Owner = owner;
      this.StateMachine = stateMachine;
      this.animationEnterName = animationEnterName;
    }

    public virtual void Enter() {
      //Owner.Animator.SetBool(animationEnterName, true);
      triggerCalled = false;
    }
    public virtual void Exit() {
      //Owner.Animator.SetBool(animBoolName, false);
    }
    public virtual void Update() {
      
    }

    public virtual void AnimationFinishedTrigger() {
      triggerCalled = true;
    }

  }
}
