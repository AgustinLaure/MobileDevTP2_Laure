using UnityEngine;

public static class AnimationUtils
{
    public static bool GetAnimationEnded(Animator animator, int currentAnimHash)
    {
        AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);

        return animatorInfo.normalizedTime >= 1f && animatorInfo.shortNameHash == currentAnimHash;
    }

    public static float GetCurrentAnimationNormalizedTime(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static float GetCurrentAnimationLength(Animator animator)
    {
        return animator.GetCurrentAnimatorClipInfo(0).Length;
    }
}
