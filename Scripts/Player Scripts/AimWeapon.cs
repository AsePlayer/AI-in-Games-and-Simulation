using System.Collections;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected string attackAnimationName;
    [SerializeField] private bool spawnProjectile;
    [SerializeField] private bool isEnemy;

    public Transform aimTransform;
    public Transform aimGunEndPointTransform;
    
    private GameObject gunObject;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Gun gun;

    private GameObject meleeObject;
    [SerializeField] private Transform meleeTransform;
    [SerializeField] private Melee melee;

    public float angle;

    public AiAgent agent;

    private GameObject player;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        animator = gameObject.transform.root.GetComponent<Animator>();
        agent = gameObject.transform.GetComponent<AiAgent>();
        player = GameObject.Find("Player");

        // Cache gun information
        gunTransform = transform.Find("Aim/RightArm/Gun");
        if (gunTransform != null && gunTransform.gameObject.activeSelf)
            gun = gunTransform.GetComponent<Gun>();

        // Cache melee information
        meleeTransform = transform.Find("Aim/RightArm/Melee");
        if (meleeTransform != null && meleeTransform.gameObject.activeSelf)
            melee = meleeTransform.GetComponent<Melee>();


        // Determine if Player or Enemy.
        if (gameObject.layer != LayerMask.NameToLayer("Player"))
            isEnemy = true;
    }


    private void Update()
    {
        handleAiming();
        handleShooting();
        handleReloading();
    }

    private void handleAiming()
    {
        // Handle player aiming
        if(!isEnemy)
        {
            Vector3 mousePosition = getMouseWorldPosition();

            Vector3 aimDirection = (mousePosition - transform.position).normalized;
            // Getting Euler Angle (we want a fixed x and y coordinate system, with the z-axis being the only one rotating for 2D).
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //Source: https://youtu.be/fuGQFdhSPg4?t=275
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
        }
        // Handle enemy aiming
        else
        {
            int layer = LayerMask.GetMask("Impassable");
            int playerx = Mathf.RoundToInt(player.transform.position.x);
            int playery = Mathf.RoundToInt(player.transform.position.y);
            int enemyx = Mathf.RoundToInt(gameObject.transform.position.x);
            int enemyy = Mathf.RoundToInt(gameObject.transform.position.y);
            float rayDist = Mathf.Sqrt((playerx - enemyx) * (playerx - enemyx) + (playery - enemyy) * (playery - enemyy));

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(enemyx, enemyy), new Vector2(playerx - enemyx, playery - enemyy), rayDist, layer);
            if (hit.collider != null && hit.collider.gameObject.tag == "Wall")
            {
                Debug.DrawRay(new Vector2(playerx, playery), new Vector2(enemyx - playerx, enemyy - playery), Color.red, 0.02f);
            }
            else
            {
                Debug.DrawRay(new Vector2(playerx, playery), new Vector2(enemyx - playerx, enemyy - playery), Color.blue, 0.02f);
                Vector3 aimDirection = (agent.target.position - agent.t.position).normalized;
                var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                agent.transform.eulerAngles = new Vector3(0, 0, angle);
            }
        }

        if (gun != null)
            animator.SetBool("hasGun", true);
        else
            animator.SetBool("hasGun", false);
        if (melee != null)
            animator.SetBool("hasMelee", true);
        else
            animator.SetBool("hasMelee", false);
    }

    private void handleShooting()
    {
        
        Vector3 gunEndPointPosition = aimGunEndPointTransform.position;

        if (Input.GetMouseButtonDown(0) && !isEnemy)
        {
            Vector3 mousePosition = getMouseWorldPosition();

            Vector3 shootPosition = mousePosition;

            Vector3 aimDirection = (mousePosition - transform.position).normalized;
            
            // Getting Euler Angle (we want a fixed x and y coordinate system, with the z-axis being the only one rotating for 2D).
            angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //Source: https://youtu.be/fuGQFdhSPg4?t=275


            // If gun is not null, shoot.
            if (gun != null)
                gun.shoot(gunEndPointPosition, angle, aimDirection);

            else if (melee != null)
            {
                melee.swing(gunEndPointPosition, angle, aimDirection);
                StartCoroutine(waitToSpawnHitbox());

            }
        }
        if(isEnemy)
        {
            Vector3 aimDirection = (agent.target.position - agent.t.position).normalized;
            var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            agent.transform.eulerAngles = new Vector3(0, 0, angle);


            // If gun is not null, shoot.
            if (gun != null)
                gun.shoot(gunEndPointPosition, angle, aimDirection);

            else if (melee != null)
            {
                if (melee.weaponOnCooldown)
                    return;
                melee.swing(gunEndPointPosition, angle, aimDirection);
                StartCoroutine(waitToSpawnHitbox());
            }
        }
    }

    private void handleReloading()
    {
        // Request manual reload if R is pressed.
        if(Input.GetKeyDown(KeyCode.R) && !isEnemy)
        {
            if(gun != null)
            {
                gun.reload();
            }
        }
    }

    public void setWeaponAnimationStatus(int status)
    {
        animator.SetBool(attackAnimationName, System.Convert.ToBoolean(status));
    }
    public void setWeaponAnimationStatus(string name, int status)
    {
        animator.SetBool(name, System.Convert.ToBoolean(status));
    }

    public void setAnimation(string name)
    {
        attackAnimationName = name;
    }

    public void spawnHitbox(int status)
    {
        spawnProjectile = System.Convert.ToBoolean(status);
    }

    public IEnumerator waitToSpawnHitbox()
    {
        yield return new WaitUntil(() => spawnProjectile == true);
        if (melee != null)
            melee.setSpawnProjectile(true);
        spawnProjectile = false;
    }


    // Helper functions
    public static Vector3 getMouseWorldPosition()
    {
        Vector3 vec = getMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }

    public static Vector3 getMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
