using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudgieController : MonoBehaviour {

    static System.Random rand = new System.Random(System.DateTime.Now.Millisecond);

    private CharacterController charController;
    private float maxSpeedY = 5;
    private float speedY;
    private float gravity = 1.0f;
    private Animator animator;

    private float beginJumpTimeout;
    public float walkSpeed;
    public float runSpeed;
    public float rotationSpeed;
    public float jumpSpeed;
   
    public GameObject followCamera;
    
    public float distToGround;
    private bool performJump;

    
    public float jumpTimeOut=1;
    private float lastJumpTime = -100;
    private Vector3 preJumpMovement = Vector3.zero;
    private float jumpBoostFactor = 1.5f;

    private Material[] materials;
    private Color[] original_material_color;
    private Color hitColor = new Color(1, 1, 1, 1);

    private bool hit = false;
    private float hitTime;
    private float hitCoolDown = 1.0f;
    public float hitSpeed;
    private Vector3 hitJump;

    private Vector3 spawnpoint;


    // Use this for initialization
    void Start () {
        speedY = 0;
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetBool("isIdle", true);

        materials = GetComponentsInChildren<Renderer>()[0].materials;
        original_material_color = new Color[materials.Length];
        for(int i = 0; i < original_material_color.Length; i++)
        {
            original_material_color[i] = materials[i].color;
        }
        // doesn't work fix
        //distToGround = charController.bounds.extents.y;



    }
	
   
    private void enemyContact(Collider other)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Debug.Log("killed enemy");
            other.gameObject.GetComponent<BeeController>().initDeath();
        }
        else
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = hitColor;
            }
            // set time of hit
            hitTime = Time.time;
            // set hit flag
            hit = true;
            // randomize hit "jump"
            hitJump = new Vector3((float)rand.NextDouble()+0.5f, 1, (float)rand.NextDouble() + 0.5f);

            Debug.Log("hit by enemy");
        }
    }

    private void hitMovement()
    {
        if (Time.time - hitTime > hitCoolDown)
        {
            // hit movement is finished
            hit = false;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = original_material_color[i];
            }
        }
        else
        {
            // do something
            charController.Move(hitJump * hitSpeed *Time.deltaTime);
        }
    }

    public void boneTriggered(Collider other)
    {   
        if (other.gameObject.CompareTag("enemy"))
        {
            enemyContact(other);
        }
    }
    bool isStationary()
    {
        Vector3 velocity = charController.velocity;
        // we only care about xz axis
        velocity.y = 0;
        //why 0.01 ? just took a reasonable sounding value
        return velocity.sqrMagnitude < 0.25;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if(hit)
        {
            hitMovement();
        } else
        {
            // normal movement
            updateMovement();
        }
        

    }

    private void updateMovement()
    {
        Vector3 movementToUse = new Vector3(0, speedY, 0);
        //Debug.DrawRay(transform.position, transform.up*10, Color.green, 0.1f);
        //Debug.DrawRay(transform.position, transform.forward, Color.blue, 0.1f);
        //Debug.DrawRay(transform.position, -transform.up* distToGround, Color.black, 0.1f);
        //Debug.DrawRay(transform.position, Quaternion.Euler(0, 90, 0) * transform.forward, Color.yellow, 0.1f);

        // gravity
        applyGravity();
        Vector3 directionToCamera = followCamera.transform.position - transform.position;
        directionToCamera.y = 0;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            //the character should rotate in the z axis when the player presses the movement keys. The rotation can be calculated as
            float rotateChar = Mathf.Atan2(moveVertical, -moveHorizontal) / Mathf.PI * 180 - 90;
            // We should rotate the character relative the cameras forward vector. We should the Slerp function to slowly rotate
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Quaternion.Euler(0, rotateChar, 0) * directionToCamera), rotationSpeed * Time.deltaTime);

            Vector3 inputMovement = new Vector3(moveHorizontal, speedY, moveVertical);
            inputMovement = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * inputMovement;

            if (isStationary())
            {
                // if we are standing still then we should start moving the character when the characters rotation is close enough to the desired rotation.
                // The difference in angle to the desired rotation can be calculated as follows
                float angleDiff = Quaternion.Angle(Quaternion.LookRotation(Quaternion.Euler(0, rotateChar, 0) * directionToCamera), Quaternion.LookRotation(transform.forward));

                // what is reasonable? 15 degrees for now
                if (angleDiff < 15.0f)
                {
                    movementToUse += inputMovement;
                }
            }
            else
            {
                // if we are not standing still then we should rotate while moving forward
                movementToUse += inputMovement;

            }


        }

        // multiply with speed depending on running/walking/falling
        if (!IsGrounded())
        {
            movementToUse.x = preJumpMovement.x;
            movementToUse.z = preJumpMovement.z;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            movementToUse.x = movementToUse.x * runSpeed;
            movementToUse.z = movementToUse.z * runSpeed;
        }
        else
        {
            movementToUse.x = movementToUse.x * walkSpeed;
            movementToUse.z = movementToUse.z * walkSpeed;
        }

        // lets apply our movement
        charController.Move(movementToUse * Time.deltaTime);
    }

    void applyGravity()
    {
        //should probably have some time out when you jump
        if (!IsGrounded())
        {
            speedY -= gravity;
        } else if(Time.time - lastJumpTime > jumpTimeOut)
        {
            // if we have been grounded and the jump key was not pressed in the interval between now and now - timeout.
            // Why, well the updates after the jump the character will still be grounded and then the speed would be set to zero and the jump would be cancelled.
            speedY = -1;
            preJumpMovement = Vector3.zero;
        }
    }

    bool IsGrounded()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * distToGround, Color.white);
        Debug.DrawRay(transform.position + new Vector3(0.1f, 0, 0), transform.TransformDirection(-Vector3.up) * distToGround, Color.white);
        Debug.DrawRay(transform.position + new Vector3(-0.1f, 0, 0), transform.TransformDirection(-Vector3.up) * distToGround, Color.white);
        Debug.DrawRay(transform.position + new Vector3(0, 0, 0.1f), transform.TransformDirection(-Vector3.up) * distToGround, Color.white);
        Debug.DrawRay(transform.position + new Vector3(0, 0, -0.1f), transform.TransformDirection(-Vector3.up) * distToGround, Color.white);
        // to make it less likely to fail due lets send out a number of raycasts and return true if any hits
        return Physics.Raycast(transform.position + new Vector3(0, 0, 0), -Vector3.up, distToGround + 0.25f) ||
               Physics.Raycast(transform.position + new Vector3(0.1f, 0, 0), -Vector3.up, distToGround + 0.25f) ||
               Physics.Raycast(transform.position + new Vector3(-0.1f, 0, 0), -Vector3.up, distToGround + 0.25f) ||
               Physics.Raycast(transform.position + new Vector3(0, 0, 0.1f), -Vector3.up, distToGround + 0.25f) ||
               Physics.Raycast(transform.position + new Vector3(0, 0, -0.1f), -Vector3.up, distToGround + 0.25f);


    }

    void jump()
    {
        speedY = jumpSpeed;
        lastJumpTime = Time.time;

        // the idea is that the movement in xz axis should remain during the jump
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        preJumpMovement = new Vector3(moveHorizontal, 0, moveVertical);
        preJumpMovement = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * preJumpMovement;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            preJumpMovement = preJumpMovement * runSpeed* jumpBoostFactor;
        }
        else
        {
            preJumpMovement = preJumpMovement * walkSpeed* jumpBoostFactor;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastJumpTime > jumpTimeOut && IsGrounded())
        {
            
            animator.SetBool("isJumping", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
            jump();



        } else if (Input.GetKeyDown(KeyCode.LeftControl)) {
            animator.SetBool("isJumping", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
        }
        else if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            } else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            animator.SetBool("isJumping", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);
        } else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }

        
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pick_up"))
        {
            // Destroy(other.gameObject);
            pickUpController script = other.gameObject.GetComponent<pickUpController>();
            script.PlayPickUpSound();
            //score = score + 1;
            //setScoreText();
            //other.gameObject.SetActive(false);

            //PickUpAudioSource.Play();
        }
        else if (other.gameObject.CompareTag("heart"))
        {
            pickUpController script = other.gameObject.GetComponent<pickUpController>();
            script.PlayPickUpSound();
        }
        else if (other.gameObject.CompareTag("map_teleport"))
        {
            TeleportController script = other.gameObject.GetComponent<TeleportController>();
            script.changeMap();
        }
        else if (other.gameObject.CompareTag("enemy"))
        {
            enemyContact(other);
        }
        else if (other.gameObject.CompareTag("checkpoint"))
        {
            spawnpoint = other.gameObject.GetComponent<CheckpointController>().GetSpawnPoint();
        }
        else if (other.gameObject.CompareTag("water"))
        {
            transform.position = spawnpoint;
        }
    }

}


