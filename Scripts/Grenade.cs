using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public int damage;
    public GameObject player;
    public Database database;
    public Sprite boom;
    public float speed;

    float timer;
    bool landed;
    bool exploded;
    int startHP;
    Vector2 destination;

    int offset;
    int distance;
    float score;
    bool record;

    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.Find("Database").GetComponent<Database>();
        startHP = player.GetComponent<Health>().getHealth();
        timer = 1.0f;
        landed = false;
        exploded = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!landed)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, destination, speed * Time.deltaTime);
            if (((Vector2)gameObject.transform.position - destination).magnitude < 0.01)
            {
                landed = true;
            }
        }
        else if (!exploded)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                exploded = true;

                Vector2 grnd = gameObject.transform.position;

                //Gets all of the bruhs caught in the explosion and damages them all
                Collider2D[] explosion = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(3, 3), 0, LayerMask.GetMask("Player") | LayerMask.GetMask("Enemy"));
                for (int i = 0; i < explosion.Length; i++)
                {
                    Vector2 hit = explosion[i].gameObject.transform.position;
                    RaycastHit2D ray = Physics2D.Raycast(grnd, hit - grnd, (hit-grnd).magnitude, LayerMask.GetMask("Impassable"));
                    if (ray.collider == null || ray.collider.gameObject.name != "Wall(Clone)")
                    {
                        explosion[i].gameObject.GetComponent<Health>().takeDamage(damage);

                        //Hitting the player is very good!
                        //Hitting the enemy is mildly bad.
                        if (ray.collider != player)
                        {
                            score -= 5;
                        }
                        else
                        {
                            score += 100;
                        }
                    }
                }
                //Points for if grenade at least indirectly caused harm to player.
                if (player.GetComponent<Health>().getHealth() < startHP)
                {
                    score += startHP - player.GetComponent<Health>().getHealth();
                }
                //Won't record data if there was no lead to be made
                if (record)
                {
                    database.addData(offset, distance, score - 1);
                }

                gameObject.GetComponent<SpriteRenderer>().sprite = boom;
                gameObject.transform.localScale *= 2;

                timer = 0.5f;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void setInfo(Vector2 d, int off, int dist, bool rec)
    {
        destination = d;
        offset = off;
        distance = dist;
        record = rec;
    }
}
