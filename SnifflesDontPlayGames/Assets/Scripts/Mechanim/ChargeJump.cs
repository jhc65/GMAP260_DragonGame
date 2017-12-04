using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeJump : StateMachineBehaviour {

	public float minPower = .5f;
	public float maxPower = 10f;
	private float power;

	 //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

	}

	void ReleaseJump(Animator animator) {
		
		// If no direction is specified after (maxPower) seconds, just jump in facing direction
		if (power >= maxPower) {
		//	Debug.Log("Jumping with " + power);
			animator.SetFloat("JumpPower", power);
			animator.SetInteger("JumpDir", animator.GetInteger("Dir"));
			return;
		}

		// Jump in desired direction
		float horiz = Input.GetAxis("Horizontal");
		float vert = Input.GetAxis("Vertical");

		// Left requested
		if (horiz < 0) {
		//	Debug.Log("Jumping with " + power);
			animator.SetFloat("JumpPower", power);
			animator.SetInteger("JumpDir", 0);
		}

		// Right requested
		else if (horiz > 0) {
		//	Debug.Log("Jumping with " + power);
			animator.SetFloat("JumpPower", power);
			animator.SetInteger("JumpDir", 1);
		}


	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (power >= minPower) {
			ReleaseJump(animator);
		}



		// Charge up jump
		power += Time.deltaTime;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
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
