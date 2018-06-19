using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed;
    private int score;
    public Text scoreText;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        setScoreText();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.CompareTag("pick_up") )
        {
            // Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            score = score + 1;
            setScoreText();
        }
    }

    void setScoreText ()
    {
        bool checkpointReached = false;
        bool death = false;
        int prevCheckpointScore = 0;

        if(checkpointReached == true)
        {
            prevCheckpointScore = score;
        }

        if(death)
        {
            score = prevCheckpointScore;
        }


        scoreText.text = "Score: " + score.ToString();

    }
}
