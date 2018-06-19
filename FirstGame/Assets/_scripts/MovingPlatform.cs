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
                //if (Mathf.Abs(transform.position.x - goal.x) < 1 && Mathf.Abs(transform.position.y - goal.y) < 1 && Mathf.Abs(transform.position.z - goal.z) < 1)
                
                if(VectorDistanceCallculator(transform.position, goal))
                {
                    transform.position = goal;
                    number++;
                
                

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


    private bool VectorDistanceCallculator(Vector3 Vector_1, Vector3 Vector_2)
    {
        float distance = (Mathf.Sqrt(Mathf.Pow((Vector_1.x - Vector_2.x), 2) + Mathf.Pow((Vector_1.y - Vector_2.y), 2) + Mathf.Pow((Vector_1.z - Vector_2.z), 2)));
        if(distance < 0.5 && distance > -0.5)
        {
            return true;
        }

        return false;
    }

   
}
