using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerAdvanced : MonoBehaviour {

    public Transform playerTransform;
    public Transform cameraTransform;

    private float distance = 3.0f;
    private float currentX;
    private float currentY;
    private float sensitivityX = 1.0f;
    private float sensitivityY = 1.0f;
    private float scrollSensitivity = 1.0f;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 45.0f;
    private const float DISTANCE_MIN = 5.0f;
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
        cameraTransform.position = playerTransform.position + rotation * dir;
        transform.LookAt(playerTransform.position);
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
