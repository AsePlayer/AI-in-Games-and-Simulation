using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Tower : MonoBehaviour
{

    public MapGrid map;
    public Rigidbody2D player;
    public GameObject missile;

    private float timer;
    private tile bestTile;
    private float[,] chanceTable;

    private struct tile
    {
        public int x, y;
        public float chance;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        chanceTable = new float[map.width, map.height];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //requires player to be moving to extract velocity
        if (timer >= 5 && player.velocity != Vector2.zero)
        {
            timer = 0;
            //Debug.Log("Start");
            //resets chance table
            clearChances();
            //move is set to 1 unit in the player's strongest direction
            Vector2 move;
            if (Mathf.Abs(player.velocity.x) >= Mathf.Abs(player.velocity.y))
            {
                if (player.velocity.x > 0)
                {
                    move = new Vector2(1, 0);
                }
                else
                {
                    move = new Vector2(-1, 0);
                }
            }
            else
            {
                if (player.velocity.y > 0)
                {
                    move = new Vector2(0, 1);
                }
                else
                {
                    move = new Vector2(0, -1);
                }
            }

            tile bt = new tile();
            bt.chance = -1;
            bestTile = bt;

            tile start = new tile();
            start.x = Mathf.RoundToInt(player.position.x);
            start.y = Mathf.RoundToInt(player.position.y);

            int depth = 2*(int)((player.position - (Vector2)gameObject.transform.position).magnitude / missile.GetComponent<Missile>().speed);
            if (depth > 5) depth = 5;

            //5 is a good distance
            depth = 5;

            //Debug.Log("x " + start.x + " y " + start.y);
            start.chance = 1.0f;
            calcTiles(move, start, depth);
            printTiles();
            fireMissile();
            //Debug.Log("Done");
        }
    }

    void clearChances()
    {
        for (int i = 0; i < chanceTable.GetLength(0); i++)
        {
            for (int j = 0; j < chanceTable.GetLength(1); j++)
            {
                chanceTable[i, j] = 0;
            }
        }
    }

    //adds possible tiles to list with percent chance
    void calcTiles(Vector2 move, tile t, int depth)
    {
        //adds tile probability to matching spot on map
        if (depth <= 0)
        {
            chanceTable[t.x, t.y] += t.chance;
            if (chanceTable[t.x, t.y] > chanceTable[bestTile.x, bestTile.y])
            {
                bestTile = t;
            }
            return;
        }

        float[] chances = new float[] { 0.5f, 0.2f, 0.1f, 0.2f };
        Vector2[] dirs = new Vector2[4];
        //creates all 4 possible directions
        //player's current direction is considered 'up'
        float increment = Mathf.PI / 2;
        dirs[0] = move; //up
        dirs[1] = new Vector2(dirs[0].x * Mathf.Cos(increment) - dirs[0].y * Mathf.Sin(increment), dirs[0].x * Mathf.Sin(increment) + dirs[0].y * Mathf.Cos(increment)); //right
        dirs[2] = new Vector2(dirs[0].x * Mathf.Cos(increment*2) - dirs[0].y * Mathf.Sin(increment*2), dirs[0].x * Mathf.Sin(increment*2) + dirs[0].y * Mathf.Cos(increment*2)); //down
        dirs[3] = new Vector2(dirs[0].x * Mathf.Cos(increment*3) - dirs[0].y * Mathf.Sin(increment*3), dirs[0].x * Mathf.Sin(increment*3) + dirs[0].y * Mathf.Cos(increment*3)); //left

        //calculates chances of directions (player will not move into walls)
        for (int i = 0; i < 4; i++)
        {
            if (!map.getCell(t.x + Mathf.RoundToInt(dirs[i].x), t.y + Mathf.RoundToInt(dirs[i].y)).GetComponent<MapCell>().passable)
            {
                chances[i] = 0;
                float mult = 1/chances.Sum();
                for (int j = 0; j < 4; j++)
                {
                    chances[j] *= mult;
                }
            }
        }
        
        //generates another layer, using the calculated chances
        for (int i = 0; i < 4; i++)
        {
            if (chances[i] > 0)
            {
                tile n = new tile();
                n.x = t.x + Mathf.RoundToInt(dirs[i].x);
                n.y = t.y + Mathf.RoundToInt(dirs[i].y);
                n.chance = t.chance * chances[i];

                calcTiles(dirs[i], n, depth - 1);
            }
        }
    }

    void printTiles()
    {
        //Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        
        for (int i = 0; i < chanceTable.GetLength(0); i++)
        {
            for (int j = 0; j < chanceTable.GetLength(1); j++)
            {
                GameObject current = map.getCell(i, j);
                foreach (Transform child in current.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                GameObject holder = new GameObject("text");
                holder.transform.parent = current.transform;
                holder.transform.position = map.getCell(i, j).transform.position - new Vector3(0.3f, 0, 0);
                TextMesh txt = holder.AddComponent<TextMesh>();
                txt.text = "" + decimal.Round((decimal)chanceTable[i, j], 4);
                //txt.alignment = TextAlignment.Center;
                txt.characterSize = 0.20f;
                /*txt.font = ArialFont;
                txt.material = ArialFont.material;*/
            }
        }
    }

    void fireMissile()
    {
        Vector2 dest = new Vector2(bestTile.x, bestTile.y);
        Vector2 pos = gameObject.transform.position;

        GameObject m = Instantiate(missile, pos, Quaternion.identity);
        float angle = Mathf.Atan2((dest.y - pos.y), (dest.x - pos.x)) * Mathf.Rad2Deg;
        m.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        m.GetComponent<Missile>().setDest(dest);

    }
}
