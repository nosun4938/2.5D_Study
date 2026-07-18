using UnityEngine;

public class PlayerStateMachine
{
    private Player Owner;
    private PlayerStateBase _currentState;
    public PlayerStateBase CurrentState { get { return _currentState; } }
    public PlayerStateMachine(Player player)
    {
        Owner = player;
    }

    public void ChangeState(PlayerStateBase newState)
    {
        //Debug.Log($"ChangeState: {_currentState?.GetType().Name} => {newState?.GetType().Name}");
        if (_currentState == newState)
        {
            _currentState.ReEnter();
            return;
        }

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update() => _currentState?.Update();
    public void FixedUpdate() => _currentState?.FixedUpdate();
}
