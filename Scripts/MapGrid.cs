using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MapGrid : MonoBehaviour
{
    public int width;
    public int height;
    public int goals;
    public int kits;
    public int spawnerMax;
    public int towers;

    public GameObject wall;
    public GameObject grass;
    public GameObject hole;
    public GameObject goal;
    public GameObject health;
    public GameObject enemySpawner;
    public GameObject tower;

    public Rigidbody2D player;

    public AstarPath aStar;

    enum direction {up, down, left, right};

    GameObject[,] grid;

    public MapGrid(int w, int h, int g, int k)
    {
        width = w;
        height = h;
        goals = g;
        kits = k;
    }

    public void generate(int w, int h, int g, int k)
    {
        width = w;
        height = h;
        goals = g;

        //Debug.Log(wall.GetComponent<MapCell>().id);
        grid = new GameObject[w, h];

        //Places outer walls
        for (int i = 0; i < h; i++)
        {
            if (i == 0 || i == h - 1)
            {
                for (int j = 0; j < w; j++)
                {
                    grid[j, i] = hole;
                }
            }
            else
            {
                grid[0, i] = hole;
                grid[w - 1, i] = hole;
            }
        }

        //Place start (top left)
        for (int i = 1; i < 4; i++)
        {
            for (int j = h - 4; j < h - 1; j++)
            {
                grid[i, j] = grass;
            }
        }
        player.position = new Vector2(2, h - 3);

        //Place critical rooms
        for (int i = 0; i < goals; i++)
        {
            int offsetW;
            int offsetH;
            int tries = 0;
            do
            {
                offsetW = Random.Range(2, w - 2);
                if (offsetW < (int)(2f*w/3f))
                {
                    offsetH = Random.Range(2, (int)(h/3));
                }
                else
                {
                    offsetH = Random.Range((int)(h/3), h - 2);
                }
                tries++;
            } while (!goalRadius(offsetW, offsetH, w, h) && tries < 100);

            //Debug.Log(offsetW + ", " + offsetH);
            grid[offsetW, offsetH] = goal;
        }

        //Makes random walls
        for (int i = 1; i < w - 1; i++)
        {
            //These values could lead to blocked hallways
            if (i == 2 || i == w - 3)
            {
                i++;
            }
            for (int j = 1; j < h - 1; j++)
            {
                if (j == 2 || j == h - 3)
                {
                    j++;
                }
                //if (grid[i-1,j].closed || grid[i+1,j].closed || grid[i,j-1].closed || grid[i,j+1].closed)
                //{
                    //createWall(w, h, i, j, true, new List<MapCell>());
                //}
                //else
                //{
                if (Random.Range(0, 10) > 6)
                {
                    if (checkAround(i, j))
                    {
                        createWall(w, h, i, j, false, new List<MapCell>());
                    }
                }
                //}
            }
        }

        //Fills empty spaces with floors
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (grid[i, j] == null)
                {
                    //Debug.Log("null filled");
                    grid[i, j] = grass;
                }
            }
        }
        //StartCoroutine(waitThenScan());

        //Adds health kits
        for (int i = 0; i < k; i++)
        {
            int randx;
            int randy;
            do
            {
                randx = Random.Range(4, width - 5);
                randy = Random.Range(4, height - 5);
            }
            while (!grid[randx, randy].GetComponent<MapCell>().passable);

            GameObject kit = Instantiate(health, new Vector3(randx, randy, -1), Quaternion.identity);
            kit.transform.parent = gameObject.transform;
        }

        //Adds spawners - Ryan
        for (int i = 0; i < spawnerMax; i++)
        {
            int randx;
            int randy;
            do
            {
                randx = Random.Range(2, width - 2);
                randy = Random.Range(2, height - 2);
            }
            while (!grid[randx, randy].GetComponent<MapCell>().passable);

            GameObject spawner = Instantiate(enemySpawner, new Vector3(randx, randy, -1), Quaternion.identity);
            spawner.transform.parent = gameObject.transform;
        }

        //Adds towers
        for (int i = 0; i < towers; i++)
        {
            int randx;
            int randy;
            do
            {
                randx = Random.Range(width/2, width - 2);
                randy = Random.Range(height/2, height - 2);
            }
            while (!grid[randx, randy].GetComponent<MapCell>().passable);

            GameObject t = Instantiate(tower, new Vector2(randx, randy), Quaternion.identity);
            t.transform.parent = gameObject.transform;
            t.GetComponent<Tower>().player = player;
            t.GetComponent<Tower>().map = gameObject.GetComponent<MapGrid>();
        }

        //Sets pathfinding graph size and placement
        var gg = AstarPath.active.astarData.gridGraph;
        gg.width = width;
        gg.depth = height;
        gg.center = new Vector3((width - 1) / 2f, (height - 1) / 2f, -1);
        gg.UpdateSizeFromWidthDepth();
        

        // Recalculate the graph
        AstarPath.active.Scan();
    }

    bool goalRadius(int x, int y, int w, int h)
    {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (grid[i, j] == goal)
                {
                    if (Mathf.Sqrt((x - i)*(x - i) + (y - j)*(y - j)) < 10)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    IEnumerator waitThenScan()
    {
        yield return new WaitForSeconds(1);
        Physics.SyncTransforms();
        AstarPath.active = FindObjectOfType<AstarPath>(); AstarPath.active.Scan();
    }

    void createWall(int w, int h, int x, int y, bool seal, List<MapCell> wallString)
    {
        if (grid[x, y] == null) //&& checkRadius(x, y, w, h, direction.up) && checkRadius(x, y, w, h, direction.right) && checkRadius(x, y, w, h, direction.down) && checkRadius(x, y, w, h, direction.left))
        {
            grid[x, y] = wall;

            if (Random.Range(0, 10) == 9)
            {
                return;
            }

            direction[] all = { direction.up, direction.right, direction.down, direction.left };
            List<direction> valid = new List<direction>();
            for (int i = 0; i < 4; i++)
            {
                if (checkRadius(x, y, w, h, all[i]))
                {
                    valid.Add(all[i]);
                }
            }

            //Debug.Log("Possible Directions: " + valid.Count);
            if (valid.Count > 0)
            {
                direction go = valid[Random.Range(0, valid.Count)];
                if (go == direction.up)
                {
                    //Debug.Log(go);
                    createWall(w, h, x, y - 1, seal, wallString);
                }
                else if (go == direction.right)
                {
                    //Debug.Log(go);
                    createWall(w, h, x + 1, y, seal, wallString);
                }
                else if (go == direction.down)
                {
                    //Debug.Log(go);
                    createWall(w, h, x, y + 1, seal, wallString);
                }
                else if (go == direction.left)
                {
                    //Debug.Log(go);
                    createWall(w, h, x - 1, y, seal, wallString);
                }
            }
        }
    }

    /*
    bool initialize(int w, int h, int x, int y, bool seal, List<MapCell> wallString)
    {

    }
    */

    bool checkAround(int x, int y)
    {
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (grid[i, j] == wall)
                {
                    return false;
                }
            }
        }
        return true;
    }

    bool checkRadius(int x, int y, int w, int h, direction d)
    {
        int maxh = h - 1;
        int maxw = w - 1;
        if (d == direction.up)
        {
            //Make sure we don't get an error
            if (x > 2 && x < maxw - 2 && y > 3)
            {
                //Check direction for other walls
                for (int i = x - 2; i <= x + 2; i++)
                {
                    for (int j = y - 3; j < y; j++)
                    {
                        if (grid[i, j] != null && grid[i, j].GetComponent<MapCell>().passable == false)
                        {
                            return false;
                        }
                    }
                }
                //No walls in direction!
                return true;
            }
        }
        else if (d == direction.right)
        {
            //Make sure we don't get an error
            if (x < maxw - 3 && y < maxh - 2 && y > 2)
            {
                //Check direction for other walls
                for (int i = x + 1; i < x + 3; i++)
                {
                    for (int j = y - 2; j <= y + 2; j++)
                    {
                        if (grid[i,j] != null && grid[i, j].GetComponent<MapCell>().passable == false)
                        {
                            return false;
                        }
                    }
                }
                //No walls in direction!
                return true;
            }
        }
        else if (d == direction.down)
        {
            //Make sure we don't get an error
            if (x > 2 && x < maxw - 2 && y < maxh - 3)
            {
                //Check direction for other walls
                for (int i = x - 2; i <= x + 2; i++)
                {
                    for (int j = y + 1; j < y + 3; j++)
                    {
                        if (grid[i, j] != null && grid[i, j].GetComponent<MapCell>().passable == false)
                        {
                            return false;
                        }
                    }
                }
                //No walls in direction!
                return true;
            }
        }
        else if (d == direction.left)
        {
            //Make sure we don't get an error
            if (x > 3 && y < maxh - 2 && y > 2)
            {
                //Check direction for other walls
                for (int i = x - 3; i < x; i++)
                {
                    for (int j = y - 2; j <= y + 2; j++)
                    {
                        if (grid[i, j] != null && grid[i, j].GetComponent<MapCell>().passable == false)
                        {
                            return false;
                        }
                    }
                }
                //No walls in direction!
                return true;
            }
        }
        return false;
    }

    public GameObject getCell(int x, int y)
    {
        return grid[x, y];
    }

    public void setCell(GameObject c, int x, int y)
    {
        grid[x, y] = c;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
