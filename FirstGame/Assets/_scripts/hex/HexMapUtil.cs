using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMapUtil  {

    public static List<GameObject> getLineHex(int xCoord, int yCoord, GameObject[,] hexMatrix, int sizeX, int sizeY, int distance)
    {
        List<GameObject> hexes = new List<GameObject>();

        //right left
        for (int i = 1; i <= distance; i++)
        {
            if (xCoord + i < sizeX)
            {
                hexes.Add(hexMatrix[xCoord + i, yCoord]);
            }

            if (xCoord - i >= 0)
            {
                hexes.Add(hexMatrix[xCoord - i, yCoord]);
            }
        }

        //up right
        int xTempCorr = 0;
        for (int i = 1; i <= distance; i++)
        {
            if ((yCoord+i) % 2 == 0)
            {
                xTempCorr++;
            }
            if (xCoord + xTempCorr < sizeX && yCoord + i < sizeY)
            {
                hexes.Add(hexMatrix[xCoord + xTempCorr, yCoord + i]);
            }
        }
        //up left
        xTempCorr = 0;
        for (int i = 1; i <= distance; i++)
        {
            if ((yCoord + i) % 2 == 1)
            {
                xTempCorr--;
            }
            if (xCoord + xTempCorr >= 0 && yCoord + i < sizeY)
            {
                hexes.Add(hexMatrix[xCoord + xTempCorr, yCoord + i]);
            }
        }

        //down left
        xTempCorr = 0;
        for (int i = 1; i <= distance; i++)
        {
            if ((yCoord - i) % 2 == 1)
            {
                xTempCorr--;
            }
            if (xCoord + xTempCorr >= 0 && yCoord - i >= 0)
            {
                hexes.Add(hexMatrix[xCoord + xTempCorr, yCoord - i]);
            }
        }
        //down right
        xTempCorr = 0;
        for (int i = 1; i <= distance; i++)
        {
            if ((yCoord + i) % 2 == 0)
            {
                xTempCorr++;
            }
            if (xCoord + xTempCorr < sizeX && yCoord - i >= 0)
            {
                hexes.Add(hexMatrix[xCoord + xTempCorr, yCoord - i]);
            }
        }

        return hexes;

    }

    public static List<GameObject> getNeighborHex(int xCoord, int yCoord, GameObject[,] hexMatrix, int sizeX, int sizeY, int distance, bool stopOnOccupied)
    {

        HashSet<GameObject> hexes = new HashSet<GameObject>();
        HashSet<GameObject> nextHexes = new HashSet<GameObject>();
        HashSet<GameObject> currentHexes = new HashSet<GameObject>();

        currentHexes.Add(hexMatrix[xCoord, yCoord]);
        for(int i = 0; i < distance; i++) {
            foreach (GameObject hex in currentHexes)
            {
                hexScript hexScript = hex.GetComponent<hexScript>();
                List<GameObject> neighborHexes = getNeighborHex(hexScript.getX(), hexScript.getY(), hexMatrix, sizeX, sizeY);
                foreach (GameObject neighbourHex in neighborHexes)
                {
                    if (!hexes.Contains(neighbourHex))
                    {
                        if(!stopOnOccupied || !neighbourHex.GetComponent<hexScript>().getOccupied())
                        {
                            hexes.Add(neighbourHex);
                            nextHexes.Add(neighbourHex);
                        }
                    }
                }
            }
            currentHexes.Clear();
            currentHexes.UnionWith(nextHexes);
            nextHexes.Clear();

        }

        hexes.Remove(hexMatrix[xCoord, yCoord]);
        return new List<GameObject>(hexes);
    }

    public static List<GameObject> getHexPath(int fromX, int fromY, int toX, int toY, GameObject[,] hexMatrix, int sizeX, int sizeY, int distance)
    {
        Dictionary<GameObject,GameObject> hexes = new Dictionary<GameObject,GameObject>();
        HashSet<GameObject> nextHexes = new HashSet<GameObject>();
        HashSet<GameObject> currentHexes = new HashSet<GameObject>();

        currentHexes.Add(hexMatrix[fromX, fromY]);
        for (int i = 0; i < distance; i++)
        {
            foreach (GameObject hex in currentHexes)
            {
                hexScript hexScript = hex.GetComponent<hexScript>();
                List<GameObject> neighborHexes = getNeighborHex(hexScript.getX(), hexScript.getY(), hexMatrix, sizeX, sizeY);
                foreach (GameObject neighbourHex in neighborHexes)
                {
                    if (!hexes.ContainsKey(neighbourHex))
                    {
                        if (!neighbourHex.GetComponent<hexScript>().getOccupied())
                        {
                            hexes.Add(neighbourHex, hex);
                            nextHexes.Add(neighbourHex);
                        }
                    }
                }
            }
            currentHexes.Clear();
            currentHexes.UnionWith(nextHexes);
            nextHexes.Clear();

        }

        //hexes.Remove(hexMatrix[fromX, fromY]);

        List<GameObject> path = new List<GameObject>();
        path.Add(hexMatrix[toX, toY]);
        GameObject currentHex = null;
        hexes.TryGetValue(hexMatrix[toX, toY], out currentHex);
        while (currentHex != hexMatrix[fromX, fromY])
        {
            path.Add(currentHex);
            hexes.TryGetValue(currentHex, out currentHex);
        }

        path.Reverse();
        return path;
    }

    public static List<GameObject>  getNeighborHex(int xCoord, int yCoord,GameObject[,] hexMatrix, int sizeX, int sizeY)
    {
        List<GameObject> hexes = new List<GameObject>();
        // depending on if it is a even or odd row the index behaves differently
        // perhaps change coordinate system in the future
        int rowAdjuster = 1;
        if (yCoord % 2 == 0)
        {
            rowAdjuster *= -1;
        }

        // if not on border
        if (xCoord != 0 && xCoord != sizeX - 1 &&
           yCoord != 0 && yCoord != sizeY - 1)
        {
            hexes.Add(hexMatrix[xCoord + 1, yCoord]);
            hexes.Add(hexMatrix[xCoord - 1, yCoord]);
            hexes.Add(hexMatrix[xCoord, yCoord + 1]);
            hexes.Add(hexMatrix[xCoord, yCoord - 1]);
            hexes.Add(hexMatrix[xCoord + rowAdjuster, yCoord + 1]);
            hexes.Add(hexMatrix[xCoord + rowAdjuster, yCoord - 1]);

        }
        else
        {
            // if on border do some checks
            if (xCoord + 1 < sizeX)
            {
                hexes.Add(hexMatrix[xCoord + 1, yCoord]);
            }
            if (xCoord - 1 >= 0)
            {
                hexes.Add(hexMatrix[xCoord - 1, yCoord]);
            }
            if (yCoord + 1 < sizeY)
            {
                hexes.Add(hexMatrix[xCoord, yCoord + 1]);
            }
            if (yCoord - 1 >= 0)
            {
                hexes.Add(hexMatrix[xCoord, yCoord - 1]);
            }
            if (xCoord + rowAdjuster >= 0 && xCoord + rowAdjuster < sizeX && yCoord + 1 < sizeY)
            {
                hexes.Add(hexMatrix[xCoord + rowAdjuster, yCoord + 1]);
            }
            if (xCoord + rowAdjuster >= 0 && xCoord + rowAdjuster < sizeX && yCoord - 1 >= 0)
            {
                hexes.Add(hexMatrix[xCoord + rowAdjuster, yCoord - 1]);
            }
        }

        return hexes;
    }
}
