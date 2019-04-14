using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterScript : MonoBehaviour {

    private GameObject hex;
    private BattleScript parent;
    private GameObject healthBarCanvas;

    // temp for testing
    private const float maxHp= 100;
    private float hp = maxHp;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Image[] images = healthBarCanvas.GetComponentsInChildren<Image>();
        // two images, clean up later ...
        foreach (Image image in images)
        {
            image.fillAmount = hp / maxHp;
        }
            
	}

    public void setHex(GameObject hex)
    {
        if(this.hex != null)
        {
            this.hex.GetComponent<hexScript>().setOccupied(false);
        }
        this.hex = hex;
        hex.GetComponent<hexScript>().setOccupied(true);
    }

    public GameObject getHex()
    {
        return hex;
    }

    public Vector3 movementToHex()
    {
        Vector3 hexPosition = hex.GetComponent<hexScript>().transform.position;
        return new Vector3(hexPosition.x - transform.position.x, 0, hexPosition.z - transform.position.z);
    }

    public Vector3 movementToHex(GameObject hex)
    {
        Vector3 hexPosition = hex.GetComponent<hexScript>().transform.position;
        return new Vector3(hexPosition.x - transform.position.x, 0, hexPosition.z - transform.position.z);
    }

    void OnMouseDown()
    {
        hexScript script = hex.GetComponent<hexScript>();
        parent.hexClicked(script.getX(), script.getY());
    }

    public void setParent(BattleScript parent)
    {
        this.parent = parent;
    }

    public void createHealthBarCanvas(GameObject healthBarPrefab)
    {
        float characterHeight = GetComponent<Renderer>().bounds.extents.y * 2;
        healthBarCanvas = (GameObject)(Instantiate(healthBarPrefab, transform.position + new Vector3(0, (float)(characterHeight / 2 + 0.5), 0), Quaternion.identity));
        healthBarCanvas.transform.parent = this.transform;
    }

    public void changeHealth(float healthDelta)
    {
        hp += healthDelta;
        if(hp < 0)
        {
            hp = 0;
        }
    }

    public bool isDead()
    {
        return hp == 0;
    }
}
