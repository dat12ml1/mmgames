using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {


    public Transform spawnPointTransform;
    void Start()
    {
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(spawnPointTransform.position, 0.25f);
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPointTransform.position;
    }
}
