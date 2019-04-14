using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCameraController : MonoBehaviour {

    private Transform cameraTransform;

    private float currentHeight;
    private float currentX;
    private float currentZ;
    private float sensitivityX = 0.1f;
    private float sensitivityY = 0.1f;
    private float sensitivityAngle = 1.0f;
    private float scrollSensitivity = 2.0f;

    private float startX;
    private float startZ;
    private float startHeight;

    private const float X_DELTA = 6.0f;
    private const float Z_DELTA = 1.0f;
    private const float HEIGHT_DELTA = 2.0f;

    private float currentYRotation;
    private float startYRotation;
    private const float Y_ANGLE_MAX = 20.0f;
    private const float Y_ANGLE_MIN = -20.0f;

    private float currentXRotation;
    private float startXRotation;
    private const float X_ANGLE_MAX = 50.0f;
    private const float X_ANGLE_MIN = 20.0f;


    // Use this for initialization
    void Start () {
        // set cameraTransform to the transform of the scrips gameobject (assumed main camera)
        cameraTransform = transform;
        startX = transform.position.x;
        startZ = transform.position.z;
        startHeight = transform.position.y;

        currentX = startX;
        currentZ = startZ;
        currentHeight = startHeight;
        currentYRotation = transform.rotation.eulerAngles.y;
        currentXRotation = transform.rotation.eulerAngles.x;
        Debug.Log(transform.rotation.eulerAngles);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1)){
            //right click you can move the camera in x,z axis
            currentX += -Input.GetAxis("Mouse X") * sensitivityX;
            currentX = Mathf.Clamp(currentX, startX - X_DELTA, startX + X_DELTA);
            currentZ += -Input.GetAxis("Mouse Y") * sensitivityY;
            currentZ = Mathf.Clamp(currentZ, startZ - Z_DELTA, startZ + Z_DELTA);
        } else if (Input.GetMouseButton(2))
        {
            //middle mouse button to rotate
            currentXRotation += Input.GetAxis("Mouse Y") * sensitivityAngle;
            currentXRotation = Mathf.Clamp(currentXRotation, X_ANGLE_MIN, X_ANGLE_MAX);
            currentYRotation += Input.GetAxis("Mouse X") * sensitivityAngle;
            currentYRotation = Mathf.Clamp(currentYRotation, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
        currentHeight += -Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        currentHeight = Mathf.Clamp(currentHeight, startHeight - HEIGHT_DELTA, startHeight + HEIGHT_DELTA);

    }

    private void LateUpdate()
    {
        cameraTransform.position = new Vector3(currentX, currentHeight, currentZ);
        cameraTransform.transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, cameraTransform.transform.rotation.eulerAngles.z);
    }
}
