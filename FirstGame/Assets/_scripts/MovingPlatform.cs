using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3[] wheretoGoList;

    Vector3 goal;
    int number = 0;

    private void Start()
    {
        CalulateGoal();
    }

    private void Update()
    {
        CalculateDirection();
    }

    private void CalulateGoal()
    {
        goal = transform.position + wheretoGoList[number];
    }


    

    private void CalculateDirection()
    {
        if( wheretoGoList.Length > 0)
        {            
                if (Mathf.Abs(transform.position.x - goal.x) < 1 && Mathf.Abs(transform.position.y - goal.y) < 1 && Mathf.Abs(transform.position.z - goal.z) < 1)
                {
                number++;
                transform.position = goal;

                if (number > wheretoGoList.Length - 1)
                {
                    number = 0;
                }

                CalulateGoal();
                
                }else
                {
                transform.position += wheretoGoList[number]/25 * speed * Time.fixedDeltaTime;


            }
            
                
                Debug.Log(transform.position + "______" + goal);
         }                
    }

   
}
