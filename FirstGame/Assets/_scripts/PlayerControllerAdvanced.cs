using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAdvanced : MonoBehaviour {

    public float speed;
    public GameObject followCamera;
    private Rigidbody rb;
    AudioSource RollingAudioSource;
    AudioSource CollisionAudioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var sources = GetComponents<AudioSource>();
        RollingAudioSource = sources[0];
        CollisionAudioSource = sources[1];
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
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pick_up"))
        {
            // Destroy(other.gameObject);
            pickUpController script=  other.gameObject.GetComponent<pickUpController>();
            script.PlayPickUpSound();
            //other.gameObject.SetActive(false);
            
            //PickUpAudioSource.Play();
        } else if(other.gameObject.CompareTag("map_teleport"))
        {
            TeleportController script = other.gameObject.GetComponent<TeleportController>();
            script.changeMap();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 3)
        {
            CollisionAudioSource.volume = collision.relativeVelocity.magnitude / 40;
            CollisionAudioSource.Play();
        }
            
    }
}
