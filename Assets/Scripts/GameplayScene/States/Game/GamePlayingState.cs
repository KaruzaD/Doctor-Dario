
using GDEUtils.StateMachine;


public class GamePlayingState : State<GameManager> {
  public GamePlayingState(GameManager owner, StateMachine<GameManager> stateMachine, string animationEnterName) : base(owner, stateMachine, animationEnterName) {
  }

  public override void Enter() {
    base.Enter();
    GameEvents.GameStarted(Owner);
  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();

    PlayerManager.Instance.HandleAllPlayerUpdates();
  }
}

