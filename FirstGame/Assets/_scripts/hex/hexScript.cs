using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexScript : MonoBehaviour {

    // Use this for initialization
    private BattleScript parent;
    private int xCoord;
    private int yCoord;
    private bool occupied = false;
   // private GameObject character;
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        parent.hexClicked(xCoord,yCoord);
    }

    public void setParent(BattleScript parent)
    {
        this.parent = parent;
    }

    public void setCoord(int xCoord, int yCoord)
    {
        this.xCoord = xCoord;
        this.yCoord = yCoord;
    }

    public int getX()
    {
        return xCoord;
    }

    public int getY()
    {
        return yCoord;
    }

    public void setOccupied(bool status )
    {
        occupied = status;
    }

    public bool getOccupied()
    {
        return occupied;
    }
}
