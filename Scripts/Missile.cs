using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    bool exploded;
    float timer;
    public float speed;
    public int damage;
    public Sprite boom;
    Vector2 destination;

    // Start is called before the first frame update
    void Start()
    {
        exploded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!exploded)
        {
            if (destination != null)
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, destination, speed * Time.deltaTime);
            }
            if (((Vector2)gameObject.transform.position - destination).magnitude < 0.01)
            {
                exploded = true;

                Vector2 grnd = gameObject.transform.position;

                //Gets all of the bruhs caught in the explosion and damages them all
                Collider2D[] explosion = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(3, 3), 0, LayerMask.GetMask("Player") | LayerMask.GetMask("Enemy"));
                for (int i = 0; i < explosion.Length; i++)
                {
                    Vector2 hit = explosion[i].gameObject.transform.position;
                    RaycastHit2D ray = Physics2D.Raycast(grnd, hit - grnd, (hit - grnd).magnitude, LayerMask.GetMask("Impassable"));
                    if (ray.collider == null || ray.collider.gameObject.name != "Wall(Clone)")
                    {
                        explosion[i].gameObject.GetComponent<Health>().takeDamage(damage);
                    }
                }
                
                gameObject.GetComponent<SpriteRenderer>().sprite = boom;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(2, 2, 1);
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void setDest(Vector2 d)
    {
        destination = d;
        Debug.Log(d);
    }
}
