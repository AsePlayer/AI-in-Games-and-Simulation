using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testerboi : MonoBehaviour
{
    /*
    public GameObject goodgridprefab;
    public GameObject badgridprefab;
    public GameObject goalgridprefab;
    public GameObject wallgridprefab;
    public GameObject crategridprefab;

    public int gridSizeX;
    public int gridSizeY;
    public int gridOffsetX;
    public int gridOffsetY;

    public int maxBad;
    public int maxGood;

    private int maxBadCounter;
    private int maxGoodCounter;
    private bool goalSpawned;

    private bool crateSpawned;

    // Start is called before the first frame update
    void Start()
    {
        GridGenerate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GridGenerate()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if ((x == gridSizeX - 1 || y == gridSizeY - 1) && !goalSpawned)
                {
                    int num = Random.Range(x, y);
                    if (x > 8)
                    {
                        Instantiate(goalgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                        goalSpawned = true;
                    }
                    else
                    {
                        Instantiate(goodgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                        maxGoodCounter++;
                    }
                }
                else if (maxGoodCounter >= maxBadCounter)
                {
                    int num = Random.Range(0, 10);
                    if (num <= 3)
                    {
                        Instantiate(goodgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                        maxGoodCounter++;
                    }
                    else if (num < 5)
                    {
                        if(!crateSpawned)
                        {
                            Instantiate(crategridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 0), Quaternion.identity);
                            Instantiate(goodgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                            crateSpawned = true;
                        }
                        else
                        {
                            Instantiate(goodgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                        }
                        maxGoodCounter--;
                    }
                    else
                    {
                        if (x < 1 || y < 1)
                        {
                            Instantiate(goodgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                            maxGoodCounter++;
                        }
                        else
                        {
                            Instantiate(badgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                            maxBadCounter++;
                        }
                    }
                }
                else if (maxGoodCounter < maxBadCounter)
                {
                    int num = Random.Range(0, 10);
                    if (num <= 5)
                    {
                        Instantiate(goodgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                        maxGoodCounter++;
                    }
                    else if (num < 7)
                    {
                        Instantiate(goodgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                        maxGoodCounter++;
                    }
                    else
                    {
                        Instantiate(badgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                        maxBadCounter++;
                    }
                }
                else
                {
                    maxGoodCounter++;
                    Instantiate(goodgridprefab, new Vector3(x + gridOffsetX, y + gridOffsetY, 1), Quaternion.identity);
                }

                // Spawn surrounding walls around the grid    
                Instantiate(wallgridprefab, new Vector3(-1 + gridOffsetX, y + gridOffsetY, 2), Quaternion.identity);
                Instantiate(wallgridprefab, new Vector3(gridSizeX + gridOffsetX, y + gridOffsetY, 2), Quaternion.identity);

            }
            maxGoodCounter--;

            // Spawn surrounding walls around the grid         
            Instantiate(wallgridprefab, new Vector3(1 + x + gridOffsetX, -1 + gridOffsetY, 2), Quaternion.identity);
            Instantiate(wallgridprefab, new Vector3(-1 + x + gridOffsetX, -1 + gridOffsetY, 2), Quaternion.identity);
            Instantiate(wallgridprefab, new Vector3(1 + x + gridOffsetX, gridSizeY + gridOffsetY, 2), Quaternion.identity);
            Instantiate(wallgridprefab, new Vector3(-1 + x + gridOffsetX, gridSizeY + gridOffsetY, 2), Quaternion.identity);
        }
                
    }
    */

}
