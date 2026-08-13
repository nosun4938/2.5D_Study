using UnityEngine;

public class MonsterStateMachine
{
    private Monster Owner;
    private MonsterStateBase _currentState;
    public MonsterStateBase CurrentState { get { return _currentState; } }
    public MonsterStateMachine(Monster monster)
    {
        Owner = monster;
    }

    public void ChangeState(MonsterStateBase newState)
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
