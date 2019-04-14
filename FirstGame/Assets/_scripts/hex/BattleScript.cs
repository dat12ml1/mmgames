using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScript : MonoBehaviour {

    public GameObject hexMapPrefab;
    private GameObject hexMap;
    public int mapSizeX;
    public int mapSizeY;

    public GameObject[] prefabCharacters;
    public int[] characterXPositions;
    public int[] characterYPositions;

    public float gamespeed;
    public int movementDistance;

    private Queue<GameObject> queue;
    private List<GameObject> instantiatedCharacters;
    private GameObject activeCharacter;

    private List<GameObject> movementHexOrder;
    private int movementHexOrderIndex;
    private enum GameState { waiting, moving, nextCharacter};
    private GameState currentState = GameState.waiting;

    void Start () {
        initializeMap();
        initializeCharacterQueue();
    }

    private void initializeMap()
    {
        hexMap = (GameObject)(Instantiate(hexMapPrefab, transform.position, transform.rotation));
        hexMap.GetComponent<hexMapScript>().setMapSize(mapSizeX, mapSizeY);
        hexMap.GetComponent<hexMapScript>().createMap(this);
        instantiatedCharacters =  hexMap.GetComponent<hexMapScript>().addCharacters(prefabCharacters, characterXPositions, characterYPositions,this);
    }

    private void initializeCharacterQueue()
    {
        queue = new Queue<GameObject>();
        for(int i = 0; i < instantiatedCharacters.Count; i++)
        {
            queue.Enqueue(instantiatedCharacters[i]);
        }
    }

    // Update is called once per frame
    void Update () {

        switch (currentState)
        {
            case GameState.waiting:
                if (activeCharacter != queue.Peek())
                {
                    // new active characters! lets display the available movies
                    activeCharacter = queue.Peek();
                    // color the surrounding hexes
                    colorNeighboringHexes(Color.green, Color.cyan, Color.red);
                }
                break;

            case GameState.moving:
                //TODO fix so that it moves hex to hex instead of shortest path
                Vector3 movement = activeCharacter.GetComponent<characterScript>().movementToHex(movementHexOrder[movementHexOrderIndex]);
                activeCharacter.GetComponent<CharacterController>().Move(movement.normalized * Time.deltaTime * gamespeed);
                if (movement.magnitude < 0.1)
                {
                    movementHexOrderIndex++;
                    if (movementHexOrderIndex == movementHexOrder.Count)
                    {
                        currentState = GameState.nextCharacter;
                    }
                }
                break;
            case GameState.nextCharacter:
                queue.Enqueue(activeCharacter);
                queue.Dequeue();
                currentState = GameState.waiting;
                break;
        }
            
        

    }

    public void hexClicked(int xCoord, int yCoord)
    {
        // A hex is pressed
        switch (currentState)
        {
            case GameState.waiting:
                GameObject pressed_hex = hexMap.GetComponent<hexMapScript>().getHex(xCoord, yCoord);

                //is the pressed hex a hex the active character can move to?
                GameObject active_hex = activeCharacter.GetComponent<characterScript>().getHex();
                List<GameObject> hexNeighbors = hexMap.GetComponent<hexMapScript>().getHexNeighbor(active_hex, movementDistance);
                if (hexNeighbors.Contains(pressed_hex))
                {
                    //yes! Start by resetting the colors on the surrounding hexes
                    colorNeighboringHexes(Color.yellow, Color.yellow, Color.yellow);
                    //save movement list of hexes to walk on
                    movementHexOrder = hexMap.GetComponent<hexMapScript>().getHexPath(activeCharacter.GetComponent<characterScript>().getHex(), pressed_hex);
                    movementHexOrderIndex = 0;
                    //now change the hex the character is on
                    activeCharacter.GetComponent<characterScript>().setHex(pressed_hex);
                    // the character still needs to move in the update method
                    currentState = GameState.moving;

                }
                else
                {
                    // is the pressed hex a hex with an enemy that the active character can attack?
                    hexNeighbors = hexMap.GetComponent<hexMapScript>().getHexNeighborEnemies(active_hex);
                    if (hexNeighbors.Contains(pressed_hex))
                    {
                        //yes! Start by resetting the colors on the surrounding hexes
                        colorNeighboringHexes(Color.yellow, Color.yellow, Color.yellow);
                        //attack! For now just remove enemy
                        attackEnemyOnHex(pressed_hex);
                        //end turn
                        currentState = GameState.nextCharacter;
                    }
                }

                
                    break;
        }

    }

    private void colorNeighboringHexes(Color activeHexColor,Color walkableHexColor, Color enemyHexColor)
    {
        GameObject active_hex = activeCharacter.GetComponent<characterScript>().getHex();
        active_hex.GetComponent<Renderer>().material.color = activeHexColor;
        List<GameObject> hexNeighbors = hexMap.GetComponent<hexMapScript>().getHexNeighbor(active_hex, movementDistance);
        foreach (GameObject hexNeigbor in hexNeighbors)
        {
            hexNeigbor.GetComponent<Renderer>().material.color = walkableHexColor;
        }
        //color hexes with enemies you can attack red
        hexNeighbors = hexMap.GetComponent<hexMapScript>().getHexNeighborEnemies(active_hex);
        foreach (GameObject hexNeigbor in hexNeighbors)
        {
            hexNeigbor.GetComponent<Renderer>().material.color = enemyHexColor;
        }
    }

    private void attackEnemyOnHex(GameObject hex){
        GameObject character = null;
        for(int i = 0; i < instantiatedCharacters.Count; i++)
        {
            if(instantiatedCharacters[i].GetComponent<characterScript>().getHex() == hex)
            {
                character = instantiatedCharacters[i];
            }
        }
        character.GetComponent<characterScript>().changeHealth(-25.0f);
        if (character.GetComponent<characterScript>().isDead())
        {
            character.GetComponent<Renderer>().material.color = Color.red;
        }

    }


}
