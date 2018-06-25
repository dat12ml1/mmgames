using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerAdvanced : MonoBehaviour {

    static System.Random rand = new System.Random(System.DateTime.Now.Millisecond);
    public float speed;
    public GameObject followCamera;
    private Rigidbody rb;
    AudioSource RollingAudioSource;
    AudioSource CollisionAudioSource;
    public Vector3 jumpForce;
    private Collider collider;

    float distToGround;
    bool performJump;

    //text
    private int score;
    public Text scoreText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var sources = GetComponents<AudioSource>();
        
        RollingAudioSource = sources[0];
        CollisionAudioSource = sources[1];

        distToGround = GetComponent<Collider>().bounds.extents.y;

        //text
        score = 0;
        setScoreText();
        collider = GetComponent<Collider>();
    }

    public Collider GetPlayerCollider()
    {
        return collider;
    }

    bool IsGrounded()  {
       return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
     }

/*
 *The idea is that the movement of the player should depend on the camera. So forward should be
 * the direction the camera is facing
 */
void FixedUpdate()
    {
        /* 
         * Set up movement vector just as for the simple PlayerController
         * 
        */
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        Vector3 xzPlaneVelocity = rb.velocity;
        xzPlaneVelocity.y = 0;
        RollingAudioSource.volume = xzPlaneVelocity.sqrMagnitude / 100;
        /*
         * The Euler function will return a struct that multiplied with a movement vector
         * will rotate the vector according to the euler functions input. Exactly how does
         * this work in the background? not sure yet, magic! What we want to do here is
         * to rotate the y axis (top down rotation) of the movement according to the cameras
         * rotation. The cameras rotation in the y axis can be fetched through its transform,
         * Neat!
         */
        movement = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * movement;

        // You can only jump when on the ground
        if (IsGrounded())
        {
            
            if (performJump)
            {
                performJump = false;
                rb.AddForce(jumpForce, ForceMode.Impulse);
            }
        } else
        {
            // You can move in the xz axis when not grounded however your movement is decreased
            movement = movement / 2;
        }

        rb.AddForce(movement * speed);

            
    }

    void Update()
    {
        if(!performJump)
        {
            performJump = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pick_up"))
        {
            // Destroy(other.gameObject);
            pickUpController script=  other.gameObject.GetComponent<pickUpController>();
            script.PlayPickUpSound();
            score = score + 1;
            setScoreText();
            //other.gameObject.SetActive(false);

            //PickUpAudioSource.Play();
        } else if(other.gameObject.CompareTag("map_teleport"))
        {
            TeleportController script = other.gameObject.GetComponent<TeleportController>();
            script.changeMap();
        }
        else if (other.gameObject.CompareTag("enemy"))
        {
            Debug.Log("collision!");
            rb.AddForce(jumpForce + new Vector3((float) rand.NextDouble(), (float) rand.NextDouble(), (float)rand.NextDouble()), ForceMode.Impulse);
        }
    }

    public int sound_impulse_threshold;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > sound_impulse_threshold)
        {
            CollisionAudioSource.volume = collision.relativeVelocity.magnitude / 40;
            CollisionAudioSource.Play();
            
        }
    }

    void setScoreText()
    {
        bool checkpointReached = false;
        bool death = false;
        int prevCheckpointScore = 0;

        if (checkpointReached == true)
        {
            prevCheckpointScore = score;
        }

        if (death)
        {
            score = prevCheckpointScore;
        }


        scoreText.text = "Score: " + score.ToString();

    }
}
