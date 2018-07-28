using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class BeeController : MonoBehaviour {

    static System.Random rand = new System.Random(System.DateTime.Now.Millisecond);

    private CharacterController charController;
    private float preferredDistanceToGround;
    public float detectDistance;
    public float speed;

    public GameObject player;
    private Transform playerTransform;
    private float hysteres = 0.01f;
    private float gravity_speed = 1.0f;

    private bool alive=true;
    private float deathTime;
    private float deathTimeOut=1.0f;
    private Vector3 deathJump;

    Vector3 directionToPlayer;
    void Start () {
        // find out starting distance to ground
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity);
        preferredDistanceToGround = transform.position.y - hit.point.y;

        // get character controller reference
        charController = GetComponent<CharacterController>();

        // Set up so that physics is ignored between bee and player
        Collider collider = GetComponent<Collider>();
        PlayerControllerAdvanced playerScript = (PlayerControllerAdvanced)player.GetComponent(typeof(PlayerControllerAdvanced));
        //Physics.IgnoreCollision(playerScript.GetPlayerCollider(), collider);

        // get transform from player gameobject
        playerTransform = player.GetComponent<Transform>();
    }
	
    public Transform getPlayerTransform()
    {
        return player.GetComponent<Transform>();
    }

    public bool isAlive()
    {
        return alive;
    }

    void Update()
    {
        if(alive)
        {
            // if player is closer than detectDistance then the bee follow the player by rotating towards the player and moving forward
            if (Vector3.Distance(transform.position, playerTransform.position) < detectDistance)
            {
                directionToPlayer = playerTransform.position - transform.position;
                // only move in the xz axis
                directionToPlayer.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime);
                charController.Move(transform.forward * speed * Time.deltaTime);
            }
            // keep starting distance to ground by flying up/down as needed 
            followGround();
        } else
        {
            if(Time.time-deathTime > deathTimeOut)
            {
                // death scene is finished, deactivate enemy
                gameObject.SetActive(false);
            } else
            {
                // do something
                charController.Move(deathJump * Time.deltaTime);
                transform.localScale -= transform.localScale * Time.deltaTime;
            }
        }
        
    }

    public void initDeath()
    {
        // set time of death
        deathTime = Time.time;
        // set death flag
        alive = false;
        // randomize death "jump"
        deathJump = new Vector3(rand.Next(2), 2, rand.Next(1, 2));
    }
    // keep distance to ground the same
    private void followGround()
    {
        // check current distance to ground
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity);
        float currentDistance = transform.position.y - hit.point.y;

        // if distance is far apart from default distance to ground then add movement to correct it
        if (Mathf.Abs(preferredDistanceToGround - currentDistance) > hysteres)
        {
            if (currentDistance > preferredDistanceToGround)
            {
                charController.Move(new Vector3(0, -gravity_speed * Time.deltaTime, 0));
            }
            else
            {
                charController.Move(new Vector3(0, gravity_speed * Time.deltaTime, 0));
            }
        }
    }


}
