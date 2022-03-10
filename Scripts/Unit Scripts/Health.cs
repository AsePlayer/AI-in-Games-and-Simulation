using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * This script serves as a general-purpose Health module that can be added to any Object.
 * They will gain the ability to takeDamage and die, and bullets will be causing damages.
 */

public class Health : MonoBehaviour
{
    [SerializeField] protected int startingHealth = 100;
    [SerializeField] protected int maxhealth = 100;
    [SerializeField] private int health;
    [SerializeField] GameObject healthbar;
    private Image healthbarImage;
    private GameObject player;

    AiAgent agent;

    // Optimize with pooling system later
    //public event Action onDied;

    private void Awake()
    {
        health = startingHealth;
        agent = GetComponent<AiAgent>();
        player = GameObject.Find("Player");

        healthbar = Instantiate(healthbar);
        healthbarImage = healthbar.transform.Find("HP").GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 gohere = transform.position;
        gohere.y += 0.75f;
        gohere.x += 0.925f;
        
        if (healthbar != null)
        {
            healthbar.transform.position = Vector3.MoveTowards(healthbar.transform.position, gohere, 10f);
            healthbarImage.fillAmount = determineFillAmount();
        }

    }

    public void takeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Destroy(healthbar);
            die();
        }
        else if (health > maxhealth)
        {
            health = maxhealth;
        }
    }

    private void die()
    {
        // If player. Have it not destroy game object later and reset scene instead.
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // deathState.whatever to access it
        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        agent.stateMachine.ChangeState(AiStateId.Death);

        // Get enemy's score and increment player's score.
        if(this.GetComponent<Score>() != null)
            player.GetComponent<Score>().addScoreValue(this.GetComponent<Score>().getScoreValue());


        Destroy(gameObject);
        /*
         * Eventually propegate this out to Pooling system or whateva instead
         * if(onDied != null)
         * {
         *      onDied();
         * }
         */
    }

    public int getHealth()
    {
        return health;
    }

    public int getMaxHealth()
    {
        return maxhealth;
    }

    private float determineFillAmount()
    {
        //Value between 0 and 1
        return ((float)health / (float)maxhealth);
    }
}
