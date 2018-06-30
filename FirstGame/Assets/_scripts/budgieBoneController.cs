using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class budgieBoneController : MonoBehaviour {

    private Collider bone_collider;
    private BudgieController budgieController;
    void Start () {
        bone_collider = GetComponent<Collider>();
        budgieController = bone_collider.transform.root.GetComponent<BudgieController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        budgieController.boneTriggered(other);


    }
        
}