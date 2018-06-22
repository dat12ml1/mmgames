using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class BeeAnimationScript : MonoBehaviour {

    private Animator animator;
    private float preferredDistanceToGround;
    public float transitionsTime;
    public float attackDistance;
    public float detectDistance;
    public float speed;
    
    public Transform playerTransform;
    private float hysteres = 0.01f;
    private float gravity_speed = 1.0f;

    Vector3 directionToPlayer;
    void Start () {
        animator = GetComponent<Animator>();
        animator.SetBool("isFlying", true);
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity);
        preferredDistanceToGround = transform.position.y - hit.point.y;
    }
	

    void Update()
    {


        // if player is closer than detectDistance then the bee follow the player by rotating towards the player and moving forward
        if (Vector3.Distance(transform.position, playerTransform.position) < detectDistance)
        {
            directionToPlayer = playerTransform.position - transform.position;
            // only move in the xz axis
            directionToPlayer.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        
        followGround();
    }

    // keep distance to ground the same
    private void followGround()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity);
        float currentDistance = transform.position.y - hit.point.y;
        Debug.Log(preferredDistanceToGround + " " + currentDistance);
        if (Mathf.Abs(preferredDistanceToGround - currentDistance) > hysteres)
        {
            if (currentDistance > preferredDistanceToGround)
            {
                Debug.Log("go down");
                transform.Translate(0, -gravity_speed * Time.deltaTime, 0);
            }
            else
            {
                Debug.Log("go up");
                transform.Translate(0, gravity_speed * Time.deltaTime, 0);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        updateAnimation();
    }

    void updateAnimation() {
        if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
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
