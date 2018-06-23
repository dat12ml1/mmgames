using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAnimationController : MonoBehaviour {

    private Animator animator;
    //public float transitionsTime;
    public float attackAnimationDistance;
    private Transform playerTransform;

    // Use this for initialization
    void Start () {
        // get animator and starting animation as flying
        animator = GetComponent<Animator>();
        animator.SetBool("isFlying", true);
        playerTransform = GetComponent<BeeController>().getPlayerTransform();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Update is called once per frame
    void LateUpdate()
    {
        updateAnimation();
    }

    void updateAnimation()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < attackAnimationDistance)
        {
            animator.SetBool("isFlying", false);
            animator.SetBool("isAttacking", true);
            //lastAttackTime = Time.time;
            //animator.CrossFade("attack", transitionsTime);
        }
        else
        {
            animator.SetBool("isFlying", true);
            animator.SetBool("isAttacking", false);
        }
    }
}
