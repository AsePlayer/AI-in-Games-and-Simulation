using System.Collections;
using UnityEngine;
using Pathfinding;
public class AimWeapon : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] protected string attackAnimationName;
    [SerializeField] private bool spawnProjectile;
    [SerializeField] private bool isEnemy;

    public Transform aimTransform;
    public Transform aimGunEndPointTransform;
    
    private GameObject gunObject;
    [SerializeField] private Transform gunTransform;
    [SerializeField] public Gun gun;

    private GameObject meleeObject;
    [SerializeField] private Transform meleeTransform;
    [SerializeField] public Melee melee;

    public float angle;

    public AiAgent agent;

    private GameObject player;

    private bool los;
    private int layer;
    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        animator = gameObject.transform.root.GetComponent<Animator>();
        agent = gameObject.transform.GetComponent<AiAgent>();
        player = GameObject.Find("Player");

        // Cache gun information
        gunTransform = transform.Find("Aim/RightArm/GunTransform/Gun");
        if (gunTransform != null && gunTransform.gameObject.activeSelf)
            gun = gunTransform.GetComponent<Gun>();

        // Cache melee information
        meleeTransform = transform.Find("Aim/RightArm/MeleeTransform/Melee");
        if (meleeTransform != null && meleeTransform.gameObject.activeSelf)
            melee = meleeTransform.GetComponent<Melee>();


        // Determine if Player or Enemy.
        if (gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            isEnemy = true;


            // Cache gun information
            gunTransform = transform.Find("Aim/RightArm/Gun");
            if (gunTransform != null && gunTransform.gameObject.activeSelf)
                gun = gunTransform.GetComponent<Gun>();

            // Cache melee information
            meleeTransform = transform.Find("Aim/RightArm/Melee");
            if (meleeTransform != null && meleeTransform.gameObject.activeSelf)
                melee = meleeTransform.GetComponent<Melee>();
        }

        layer = LayerMask.GetMask("Impassable");
    }


    private void Update()
    {
        if (Time.timeScale == 0)
            return;
        handleChangingWeapon();
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
                los = false;

                if (GetComponent<AIPath>().hasPath)
                {

                    Vector3 nextPath = GetComponent<AIPath>().path.vectorPath[0];
                    Vector3 aimDirection = (agent.t.position - nextPath).normalized;

                    var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                    var qr = Quaternion.Euler(new Vector3(0, 0, angle));

                    agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, qr, Time.deltaTime * 5f);
                }
                    
            }
            else
            {
                Debug.DrawRay(new Vector2(playerx, playery), new Vector2(enemyx - playerx, enemyy - playery), Color.blue, 0.02f);
                Vector3 aimDirection = (agent.target.position - agent.t.position).normalized;
                var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                agent.transform.eulerAngles = new Vector3(0, 0, angle);
                los = true;
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

        if (Input.GetMouseButton(0) && gun != null && gun.isAuto && !isEnemy || Input.GetMouseButtonDown(0) && !isEnemy)
        {
            Vector3 mousePosition = getMouseWorldPosition();

            Vector3 shootPosition = mousePosition;

            Vector3 aimDirection = (mousePosition - transform.position).normalized;
            
            // Getting Euler Angle (we want a fixed x and y coordinate system, with the z-axis being the only one rotating for 2D).
            angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //Source: https://youtu.be/fuGQFdhSPg4?t=275


            // If gun is not null, shoot.
            if (gun != null)
            {
                gun.shoot(gunEndPointPosition, angle, aimDirection);
            }

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
            var qr = Quaternion.Euler(new Vector3(0, 0, angle));

            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, qr, Time.deltaTime * 5f);


            // If gun is not null and enemy is ready to attack, shoot.
            if (gun != null && los)
                gun.shoot(gunEndPointPosition, angle, aimDirection);

            // If melee is not null and enemy is in range to attack, slice.
            else if (melee != null && Vector3.Distance(agent.target.position, agent.t.position) < 1.75f)
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
        //animator.SetBool(attackAnimationName, System.Convert.ToBoolean(status));
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

    public void dropWeapon()
    {
        GameObject instance = null;
        if (gun != null)
        {
            gun.weaponOnCooldown = false;
            instance = GameObject.Instantiate(gun.gameObject);
            instance.transform.position = aimGunEndPointTransform.position;

            //Update ammo of gun on the ground with correct amount so you can't go infinite
            instance.GetComponent<Gun>().getAmmo().ammoInMag = gun.getAmmo().ammoInMag;
            //gun.transform.SetParent(null, true);
            Destroy(gun.gameObject);
            gun = null;
        }
        else if(melee != null)
        {
            melee.weaponOnCooldown = false;
            instance = GameObject.Instantiate(melee.gameObject);
            instance.transform.position = aimGunEndPointTransform.position;

            Destroy(melee.gameObject);
            melee = null;
        }

        if (isEnemy)
            Destroy(gameObject);

    }

    private void handleChangingWeapon()
    {
        // Enemy doesn't need to do this
        if (isEnemy)
            return;

        float distanceToClosestWeapon = Mathf.Infinity;
        float distanceToGun = Mathf.Infinity;
        float distanceToMelee = Mathf.Infinity;

        Gun closestGun = null;
        Melee closestMelee = null;

        Gun[] allGuns = GameObject.FindObjectsOfType<Gun>();
        Melee[] allMelees = GameObject.FindObjectsOfType<Melee>();



        foreach (Gun currentGun in allGuns)
        {
            // If the weapon isn't currently being used by a Unit (aka it has been dropped on the floor)
            if (currentGun.transform.parent == null)
            {
                distanceToGun = (currentGun.transform.position - this.transform.position).sqrMagnitude;
                if(distanceToGun < distanceToClosestWeapon)
                {
                    distanceToClosestWeapon = distanceToGun;
                    if(closestGun)
                        closestGun.gameObject.GetComponent<SpriteRenderer>().color = closestGun.color;
                    currentGun.gameObject.GetComponent<SpriteRenderer>().color = currentGun.color;
                    closestGun = currentGun;
                }
            }
        }
        foreach (Melee currentMelee in allMelees)
        {
            // If the weapon isn't currently being used by a Unit (aka it has been dropped on the floor)
            if (currentMelee.transform.parent == null)
            {
                distanceToMelee = (currentMelee.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToMelee < distanceToClosestWeapon)
                {
                    distanceToClosestWeapon = distanceToMelee;
                    if (closestMelee)
                        closestGun.gameObject.GetComponent<SpriteRenderer>().color = closestMelee.color;
                    currentMelee.gameObject.GetComponent<SpriteRenderer>().color = currentMelee.color;
                    closestMelee = currentMelee;
                }
            }
        }

        // In pickup range
        if(distanceToClosestWeapon < 2.5f)
        {
            if (distanceToGun < distanceToMelee)
            {
                Debug.DrawLine(this.transform.position, closestGun.transform.position);
                closestGun.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;

                if (Input.GetKeyDown(KeyCode.E) && closestGun)
                {
                    closestGun.gameObject.GetComponent<SpriteRenderer>().color = closestGun.color;
                    closestGun.transform.position = transform.Find("Aim/RightArm/GunTransform").position;

                    dropWeapon();

                  

                    if (gun)
                        gun.gameObject.SetActive(true);
                    if(melee)
                        melee.gameObject.SetActive(false);
                    // Cache gun information
                    
                    closestGun.transform.parent = transform.Find("Aim/RightArm/GunTransform");
                    Vector3 mousePosition = getMouseWorldPosition();

                    Vector3 aimDirection = (mousePosition - transform.position).normalized;
                    // Getting Euler Angle (we want a fixed x and y coordinate system, with the z-axis being the only one rotating for 2D).
                    float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //Source: https://youtu.be/fuGQFdhSPg4?t=275
                    closestGun.transform.eulerAngles = new Vector3(0, 0, angle);
                    closestGun.transform.name = "Gun";
                    gun = closestGun;



                }
            }
            else if(distanceToGun > distanceToMelee)
            {
                if(closestMelee)
                {
                    Debug.DrawLine(this.transform.position, closestMelee.transform.position);
                    closestMelee.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                if (Input.GetKeyDown(KeyCode.E) && closestMelee)
                {
                    closestMelee.gameObject.GetComponent<SpriteRenderer>().color = closestMelee.color;
                    closestMelee.transform.position = transform.Find("Aim/RightArm/MeleeTransform").position;
                    dropWeapon();





                    closestMelee.transform.parent = transform.Find("Aim/RightArm/MeleeTransform");
                    closestMelee.transform.name = "Melee";
                    Vector3 mousePosition = getMouseWorldPosition();

                    Vector3 aimDirection = (mousePosition - transform.position).normalized;
                    // Getting Euler Angle (we want a fixed x and y coordinate system, with the z-axis being the only one rotating for 2D).
                    float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //Source: https://youtu.be/fuGQFdhSPg4?t=275
                    closestMelee.transform.eulerAngles = new Vector3(0, 0, angle);



                    // Cache melee information
                    melee = closestMelee;
                    melee.aimWeapon = this;
                    animator.SetBool("isAttackDone", true);
                    //animator.SetBool("isReloading", false);
                    //setWeaponAnimationStatus("isAttackingMelee", 1);
                }
            }




        }
        else
        {
            //if(closestGun)
        }

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

    public bool hasWeapon()
    {
        if (gun != null || melee != null)
            return true;
        return false;
    }
}
