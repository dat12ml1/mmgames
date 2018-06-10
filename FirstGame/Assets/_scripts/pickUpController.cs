using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpController : MonoBehaviour {

    static System.Random rand = new System.Random(System.DateTime.Now.Millisecond);
    AudioSource PickUpAudioSource;
    Rigidbody rb;
    private bool pickedUpByPlayer;
    public float secondsLiveAfterPickedUp;
    private float TimePickedUp;
    // Use this for initialization
    void Start () {
        pickedUpByPlayer = false;
        PickUpAudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(pickedUpByPlayer)
        {
            if(Time.time -TimePickedUp > secondsLiveAfterPickedUp)
            {
                gameObject.SetActive(false);
            }

            
        } else
        {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }
    }

    public void PlayPickUpSound()
    {
        pickedUpByPlayer = true;
        PickUpAudioSource.Play();
        TimePickedUp = Time.time;
        rb.isKinematic = false;
        
        rb.AddForce(new Vector3(rand.Next(1, 50), 300, rand.Next(1, 50)));
    }
}
