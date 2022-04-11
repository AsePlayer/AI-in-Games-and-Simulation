using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class BruteScript : MonoBehaviour
{
    struct coord
    {
        public int x;
        public int y;
        public coord(int i, int j)
        {
            x = i;
            y = j;
        }
    }

    struct node
    {
        public int points;
        public List<coord> dir;

        public node(int p, List<coord> l)
        {
            points = p;
            dir = l;
        }
    }

    public GameObject player;
    public MapGrid map;
    public float speed;

    private bool los;
    private node optimal;

    float timer; //so that it doesn't do such a complex calculation every frame

    int layer;

    Rigidbody2D rb;

    AimWeapon aim;

    // Start is called before the first frame update
    void Start()
    {
        //path = new List<coord>();
        layer = LayerMask.GetMask("Impassable");
        timer = 1f;
        player = GameObject.Find("Player");
        map = GameObject.Find("Grid").GetComponent<MapGrid>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //if (timer >= 2.5f || optimal.dir == null || optimal.dir.Count == 0)
        //if (optimal.dir == null || optimal.dir.Count == 0)
        if (optimal.dir == null || timer >= 2.0)
        {
            int playerx = Mathf.RoundToInt(player.transform.position.x);
            int playery = Mathf.RoundToInt(player.transform.position.y);
            int enemyx = Mathf.RoundToInt(gameObject.transform.position.x);
            int enemyy = Mathf.RoundToInt(gameObject.transform.position.y);

            float rayDist = Mathf.Sqrt((playerx - enemyx) * (playerx - enemyx) + (playery - enemyy) * (playery - enemyy));
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(enemyx, enemyy), new Vector2(playerx - enemyx, playery - enemyy), rayDist, layer);
            //Rewarded for getting player in line of sight (termination state)
            if (hit.collider == null || hit.collider.gameObject.name != "Wall(Clone)")
            {
                los = true;
            }
            else
            {
                los = false;
            }
            timer = 0f;
            List<coord> best = new List<coord>();
            node n = new node(-9999, best);
            optimal = n;
            int choice = prio(playerx, playery, enemyx, enemyy, 6, 0, best);
            for (int i = 0; i < optimal.dir.Count - 1; i++)
            {
                Debug.DrawRay(new Vector2(optimal.dir[i].x, optimal.dir[i].y), new Vector2(optimal.dir[i+1].x - optimal.dir[i].x, optimal.dir[i+1].y - optimal.dir[i].y), Color.green, 0.5f);
            }

            /*string de = "";
            for (int i = 0; i < optimal.dir.Count; i++)
            {
                de += "(" + optimal.dir[i].x + "," + optimal.dir[i].y + "), ";
            }
            Debug.Log(de);*/
        }

        travel();
    }

    private int prio(int playerx, int playery, int enemyx, int enemyy, int depth, int score, List<coord> p)
    {
        //adds current point to the path
        p.Add(new coord(enemyx, enemyy));

        Health hp = gameObject.GetComponent<Health>();
        //Reward for finding health kit is greater the lower its health is (termination state)
        if (map.getCell(enemyx, enemyy).GetComponent<MapCell>().hasKit() && hp.getHealth() < hp.getMaxHealth())
        {
            //Debug.Log("Could get Kit");
            
            int final = score + (int)Mathf.Pow(0.9f, 5 - depth) * 5 * (gameObject.GetComponent<Health>().getMaxHealth() - gameObject.GetComponent<Health>().getHealth());
            //Debug.Log(final);
            node n = new node(final, p);
            if (n.points > optimal.points)
            {
                optimal = n;
                //Debug.Log("Optimal changed");
            }

            return final;
        }

        float rayDist = Mathf.Sqrt((playerx - enemyx) * (playerx - enemyx) + (playery - enemyy) * (playery - enemyy));
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(enemyx, enemyy), new Vector2(playerx - enemyx, playery - enemyy), rayDist, layer);
        //Rewarded for getting player in line of sight (termination state)
        if (hit.collider == null || hit.collider.gameObject.name != "Wall(Clone)")
        {
            //Debug.Log("Could get LOS");
            int final = score + (int)Mathf.Pow(0.7f, 5 - depth) * 50;
            node n = new node(final, p);
            if (n.points > optimal.points)
            {
                optimal = n;
                //Debug.Log("Optimal changed");
            }
            return final;
        }

        //punished for being far away
        else if (depth <= 0)
        {
            int final = score - (int)(.25*rayDist);
            node n = new node(final, p);
            if (n.points > optimal.points)
            {
                optimal = n;
                //Debug.Log("Optimal changed");
            }

            return final;
        }

        else
        {
            List<int> vals = new List<int>();
            if (map.getCell(enemyx, enemyy - 1).GetComponent<MapCell>().passable)
            {
                List<coord> next = new List<coord>(p);
                vals.Add(prio(playerx, playery, enemyx, enemyy - 1, depth - 1, score - 5, next));
            }
            if (map.getCell(enemyx - 1, enemyy).GetComponent<MapCell>().passable)
            {
                List<coord> next = new List<coord>(p);
                vals.Add(prio(playerx, playery, enemyx - 1, enemyy, depth - 1, score - 5, next));
            }
            if (map.getCell(enemyx, enemyy + 1).GetComponent<MapCell>().passable)
            {
                List<coord> next = new List<coord>(p);
                vals.Add(prio(playerx, playery, enemyx, enemyy + 1, depth - 1, score - 5, next));
            }
            if (map.getCell(enemyx + 1, enemyy).GetComponent<MapCell>().passable)
            {
                List<coord> next = new List<coord>(p);
                vals.Add(prio(playerx, playery, enemyx + 1, enemyy, depth - 1, score - 5, next));
            }
            //standing still is worse than moving
            List<coord> next2 = new List<coord>(p);
            //vals.Add(prio(playerx, playery, enemyx, enemyy, depth - 1, score - 10, next2));

            //returns the max of the list
            return vals.Max();
        }
        
    }

    private void travel()
    {
        //Debug.Log(optimal.dir.Count());
        if (optimal.dir.Count == 0 )//|| los)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        else
        {
            Vector2 goal = new Vector2(optimal.dir[0].x, optimal.dir[0].y);
            Vector2 dir = goal - rb.position;

            float mult = speed/dir.magnitude;

            // Lazy fix to NaN bug
            if(!float.IsNaN(mult) && !float.IsNaN(dir.x) && !float.IsNaN(dir.y))
                rb.velocity = mult * dir;


            var angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            var qr = Quaternion.Euler(new Vector3(0, 0, angle));

            rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, qr, Time.deltaTime * 0.5f);
        }



        if (optimal.dir.Count > 0 && (rb.position - new Vector2(optimal.dir[0].x, optimal.dir[0].y)).magnitude <= 0.05f)
        {
            optimal.dir.RemoveAt(0);
        }
    }
}

