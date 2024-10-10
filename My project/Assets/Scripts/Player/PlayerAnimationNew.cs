using System;
using UnityEngine;

public class PlayerAnimationNew : Singleton<PlayerAnimationNew>
{
    [SerializeField] Animator animatorController;

    public Animator GetAnimatorController()
    {
        return animatorController;
    }
    public void Jump()
    {
        if (animatorController.GetBool(Helper.ANIMATOR_IDLE))
        {
            //Debug.Log("Jump from idle");
            animatorController.SetTrigger(Helper.ANIMATOR_JUMP_FROM_IDLE);
            return;
        }

        if (animatorController.GetBool(Helper.ANIMATOR_STANDARD_WALK))
        {
            //Debug.Log("Jump from StandardWalk");
            animatorController.SetTrigger(Helper.ANIMATOR_JUMP_FROM_MOVING);
            return;
        }
        if (animatorController.GetBool(Helper.ANIMATOR_RUN))
        {
            //Debug.Log("Jump from Run");
            animatorController.SetTrigger(Helper.ANIMATOR_JUMP_FROM_MOVING);
            return;
        }
    }
    public void MoveBool(bool isIdle = false, bool isStandardWalk = false, bool isRun = false)
    {
        animatorController.SetBool(Helper.ANIMATOR_IDLE, isIdle);
        animatorController.SetBool(Helper.ANIMATOR_STANDARD_WALK, isStandardWalk);
        animatorController.SetBool(Helper.ANIMATOR_RUN, isRun);
    }
    public void MoveAnimation(float velocity)
    {
        animatorController.SetFloat(Helper.ANIMATOR_VELOCITY, (float)Math.Round(velocity, 1));
    }

    public void FallFlat()
    {
        animatorController.SetTrigger(Helper.ANIMATOR_FALL_FLAT);
    }

    public void ChangeAvatar(Avatar avatar)
    {
        animatorController.avatar = avatar;
    }
}