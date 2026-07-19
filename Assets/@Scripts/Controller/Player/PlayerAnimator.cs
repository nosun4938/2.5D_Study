using UnityEngine;

public class PlayerAnimator
{
    private Animator _animator;

    public string CurrentAnimation { get; private set; }
    public bool IsPlaying => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;

    public void Play(string animName)
    {
        if (CurrentAnimation == animName)
            return;

        CurrentAnimation = animName;
        _animator.Play(animName, 0, 0);
    }

    public bool isFinished()
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        return info.normalizedTime >= 1f;
    }
}
