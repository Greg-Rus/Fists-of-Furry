﻿/// <summary>
/// Place the labels for the States in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum StateID
{
    NullStateID = 0, // Use this ID to represent a non-existing State in your system
    Walking,
    FollowingAtDistance,
    Charging,
    TiedInCombat,
    Attacking,
    Punching,
    Kicking,
    WindingDownFromAttack,
    GettingHit,
    Ragdolling,
    Blocking,
    Dead,
    BlockedByWrongInput,
}