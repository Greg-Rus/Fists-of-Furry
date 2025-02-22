﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    public delegate void OnAttackCompleted();

    public OnAttackCompleted AttackCompletedDelegate;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AttackCompletedDelegate.Invoke();
    }
}