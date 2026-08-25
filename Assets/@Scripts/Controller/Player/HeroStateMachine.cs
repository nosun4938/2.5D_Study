using UnityEngine;

public class HeroStateMachine
{
    private Hero Owner;
    private HeroStateBase _currentState;
    public HeroStateBase CurrentState { get { return _currentState; } }
    public HeroStateMachine(Hero hero)
    {
        Owner = hero;
    }

    public void ChangeState(HeroStateBase newState)
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
