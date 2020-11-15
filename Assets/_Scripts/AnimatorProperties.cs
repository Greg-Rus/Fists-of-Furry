using UnityEngine;

public static class AnimatorProperties
{
    public static readonly int VeloctyX = Animator.StringToHash("Velocity X");
    public static readonly int VeloctyZ = Animator.StringToHash("Velocity Z");
    public static readonly int Charge = Animator.StringToHash("Charge");
    public static readonly int AttackSide = Animator.StringToHash("Attack Side");
    public static readonly int PunchNumber = Animator.StringToHash("Punch Number");
    public static readonly int PunchTrigger = Animator.StringToHash("Punch Trigger");
    public static readonly int KickNumber = Animator.StringToHash("Kick Number");
    public static readonly int KickTrigger = Animator.StringToHash("Kick Trigger");
    public static readonly int GetHitTrigger = Animator.StringToHash("Get Hit Trigger");
    public static readonly int HitDirection = Animator.StringToHash("Hit Direction");
    public static readonly int Blocking = Animator.StringToHash("Blocking");
    public static readonly int BlockRecoil = Animator.StringToHash("Block Recoil");
}