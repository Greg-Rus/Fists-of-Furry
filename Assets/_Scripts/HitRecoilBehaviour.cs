using UnityEngine;

public class HitRecoilBehaviour : StateMachineBehaviour
{
    public delegate void OnHitRecoilCompleted();

    public OnHitRecoilCompleted HitRecoilCompletedDelegate;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HitRecoilCompletedDelegate.Invoke();
    }
}