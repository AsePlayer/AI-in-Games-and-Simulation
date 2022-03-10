using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenadier : MonoBehaviour
{
    public GameObject player;
    public Database database;
    public GameObject grenade;
    public float firingSpeed;
    public float range;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = firingSpeed;
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Vector2 grnd = gameObject.transform.position;
            Vector2 trgt = player.transform.position;
            if ((trgt - grnd).magnitude < range)
            {
                float rnd = Random.Range(0f, 1f);
                //Chooses least done throw more often at the beginning
                if (rnd < (float)(1/(0.25*database.getTotalTries())))
                {
                    int offset = database.getLeastOffset();
                    int distance = database.getLeastDist();
                    attack(offset, distance, grnd, trgt);
                }
                //Chooses optimal throw (based on data) more often as more options have been tried
                else
                {
                    int offset = database.getBestOffset();
                    int distance = database.getBestDist();
                    attack(offset, distance, grnd, trgt);
                }
                timer = firingSpeed;
            }
            else
            {
                timer = 0.25f;
            }
        }
    }

    private void attack(int o, int d, Vector2 p, Vector2 t)
    {
        GameObject g = Instantiate(grenade, p, Quaternion.identity);
        
        if (o == 8)
        {
            g.GetComponent<Grenade>().setInfo(t, o, d, true);
        }
        else
        {
            Vector2 v = player.GetComponent<Rigidbody2D>().velocity / 3;
            float increment = 2 * Mathf.PI/8;
            float rotate = o * increment;
            Vector2 dir = new Vector2(v.x * Mathf.Cos(rotate) - v.y * Mathf.Sin(rotate), v.x * Mathf.Sin(rotate) + v.y * Mathf.Cos(rotate));
            Debug.DrawRay(t, dir, Color.magenta, 1);
            Debug.Log(v);
            if (v.x == 0 && v.y == 0)
            {
                g.GetComponent<Grenade>().setInfo(t + (dir * (d + 1)), o, d, false);
            }
            else
            {
                g.GetComponent<Grenade>().setInfo(t + (dir * (d + 1)), o, d, true);
            }
        }
    }
}
