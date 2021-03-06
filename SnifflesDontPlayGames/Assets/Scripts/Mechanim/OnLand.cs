﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLand : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		// Apply AoE damage
		animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		animator.GetComponent<PlayerController>().SpawnExplosion();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		// Reset movement and animator states
		animator.SetInteger("ChargeDir", -1);
		animator.SetInteger("JumpDir", -1);
		animator.SetFloat("JumpPower", 0.0f);
		animator.GetComponent<PlayerController>().EnableMovement();
		animator.GetComponent<PlayerController>().RemoveExplosion();
		animator.GetComponentInChildren<ShootController>().EnableShooting();
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
