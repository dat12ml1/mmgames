using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexMapScript : MonoBehaviour {

    private int sizeX;
    private int sizeY;
    public GameObject hexPrefab;
    public GameObject HealthBarCanvasPrefab;
    public float deltaX;
    public float deltaY;

    private GameObject[,] hexMatrix;
    private GameObject activeHex;

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setMapSize(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
    }

    public void createMap(BattleScript battleScript)
    {
        hexMatrix = new GameObject[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Vector3 postion;
                if (j % 2 == 1)
                {
                    postion = transform.position + new Vector3(i * deltaX + deltaX / 2, 0, j * deltaY);
                }
                else
                {
                    postion = transform.position + new Vector3(i * deltaX, 0, j * deltaY);
                }

                hexMatrix[i, j] = (GameObject)(Instantiate(hexPrefab, postion, transform.rotation));
                hexMatrix[i, j].transform.parent = this.transform;
                hexScript script = hexMatrix[i, j].GetComponent<hexScript>();
                script.setParent(battleScript);
                script.setCoord(i, j);
            }
        }

        do
        {
            int randX = (int)Random.Range(0, sizeX);
            int randY = (int)Random.Range(0, sizeY);
            hexMatrix[randX, randY].GetComponent<Renderer>().material.color = Color.yellow;
        } while (Random.value > 0.10f);
    }

    public List<GameObject> addCharacters(GameObject[] prefabCharacters, int[] xCoords, int[] yCoords, BattleScript battleScript)
    {
        if(prefabCharacters.Length != xCoords.Length || yCoords.Length!= xCoords.Length)
        {
            throw new System.ArgumentException("arrays must have the same length", "original");
        }

        List<GameObject> instantiatedCharacters = new List<GameObject>();
        for(int i = 0; i< prefabCharacters.Length; i++)
        {
            float characterHeight = prefabCharacters[i].GetComponent<Renderer>().bounds.extents.y * 2;
            GameObject character = (GameObject)(Instantiate(prefabCharacters[i], hexMatrix[xCoords[i], yCoords[i]].transform.position + new Vector3(0, characterHeight / 2, 0), Quaternion.identity));
            character.GetComponent<characterScript>().setHex(hexMatrix[xCoords[i], yCoords[i]]);
            character.GetComponent<characterScript>().setParent(battleScript);
            character.GetComponent<characterScript>().createHealthBarCanvas(HealthBarCanvasPrefab);

            instantiatedCharacters.Add(character);
        }

        return instantiatedCharacters;
    }

    public List<GameObject> getHexNeighbor(GameObject hex, int distance)
    {
        return HexMapUtil.getNeighborHex(hex.GetComponent<hexScript>().getX(), hex.GetComponent<hexScript>().getY(), hexMatrix, sizeX, sizeY, distance,true);
    }

    public List<GameObject> getHexNeighborEnemies(GameObject hex)
    {
        // a bit wasteful but lets keep it simple
        List<GameObject> hexes =  HexMapUtil.getNeighborHex(hex.GetComponent<hexScript>().getX(), hex.GetComponent<hexScript>().getY(), hexMatrix, sizeX, sizeY, 1, false);
        List<GameObject> hexesWithEnemies = new List<GameObject>();
        for(int i= 0; i < hexes.Count; i++)
        {
            if (hexes[i].GetComponent<hexScript>().getOccupied())
            {
                hexesWithEnemies.Add(hexes[i]);
            }
        }
        return hexesWithEnemies;
    }

    public GameObject getHex(int x, int y)
    {
        return hexMatrix[x, y];
    }

    public List<GameObject> getHexPath(GameObject fromHex, GameObject toHex)
    {
        // fix, remove distance
        return HexMapUtil.getHexPath(fromHex.GetComponent<hexScript>().getX(), fromHex.GetComponent<hexScript>().getY(),
                                     toHex.GetComponent<hexScript>().getX(), toHex.GetComponent<hexScript>().getY(),
                                     hexMatrix, sizeX, sizeY,100);
    }


}
