using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerAdvanced : MonoBehaviour {

    public Transform playerTransform;
    public Transform cameraTransform;

    private float distance = 4.0f;
    private float currentX;
    private float currentY;
    private float sensitivityX = 2.0f;
    private float sensitivityY = 2.0f;
    private float scrollSensitivity = 1.0f;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 45.0f;
    private const float DISTANCE_MIN = 2.5f;
    private const float DISTANCE_MAX = 10.0f;


    // Use this for initialization
    void Start () {
        // set cameraTransform to the transform of the scrips gameobject (assumed main camera)
        cameraTransform = transform;
	}
	
    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        // temp fix with vector , remove later
        cameraTransform.position = playerTransform.position+ new Vector3(0,3f,0) + rotation * dir;
        transform.LookAt(playerTransform.position + new Vector3(0, 2f, 0));
        RaycastHit hit;

        // if camera in a mesh then something should be done

        /*
         * this kind of works but i am not sure if the result is better or not ....
         * 
        if (Physics.Raycast(playerTransform.position, rotation * dir, out hit, distance))
        {
            cameraTransform.position = hit.point+  rotation * dir *-0.2f;
        }
        */
    }
    // Update is called once per frame
    void Update () {
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY += -Input.GetAxis("Mouse Y") * sensitivityY;
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        distance += -Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        distance = Mathf.Clamp(distance, DISTANCE_MIN, DISTANCE_MAX);
    }
}
