using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BUDGIE_SOUNDER_CONTROLLER;

public class test : StateMachineBehaviour {

    BudgieSoundController budgieSoundController;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    bool left = false;
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //Debug.Log((stateInfo.normalizedTime % stateInfo.length) / stateInfo.length);
        if(budgieSoundController == null)
        {
            budgieSoundController = animator.gameObject.GetComponent<BudgieSoundController>();
        }
        if((stateInfo.normalizedTime % stateInfo.length) / stateInfo.length > 0.25 && (stateInfo.normalizedTime % stateInfo.length) / stateInfo.length < 0.75 && !left)
        {
            //Debug.Log("LEFT:" + Time.time + " " + (stateInfo.normalizedTime % stateInfo.length));
            left = true;
            budgieSoundController.playClip(BUDGIE_SOUND.LEFT_LEG);
        } else if ((stateInfo.normalizedTime % stateInfo.length) / stateInfo.length > 0.75 && left)
        {
            //Debug.Log("RIGHT:" + Time.time + " " + (stateInfo.normalizedTime % stateInfo.length));
            left = false;
            budgieSoundController.playClip(BUDGIE_SOUND.RIGHT_LEG);
        }

    }

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
