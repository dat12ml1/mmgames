using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour {

    public float speed;
    public float forceX;
    public float forceY;
    public float forceZ;

    private Vector3 movement;

    void Start()
    {
        /*
         * The direction of the wind is given by the below
         * calibration params
         * */
        movement = new Vector3(forceX, forceY, forceZ);
    }

    void OnTriggerStay(Collider other)
    {
        /*
         * When a object is in the wind the objects Rigidbody should be fetched
         * and a force should be applied according to this wind objects calibration
         * params
         * */

        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(movement * speed);
    }
}
