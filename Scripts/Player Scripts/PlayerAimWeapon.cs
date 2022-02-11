using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected string attackAnimationName;
    [SerializeField] private bool spawnProjectile;

    public Transform aimTransform;
    public Transform aimGunEndPointTransform;
    
    private GameObject gunObject;
    private Gun gun;

    private GameObject meleeObject;
    private Melee melee;

    public float angle;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        animator = gameObject.transform.root.GetComponent<Animator>();

        // Cache gun information
        gunObject = GameObject.Find("Gun");
        if (gunObject != null)
            gun = gunObject.GetComponent<Gun>();

        // Cache melee information
        meleeObject = GameObject.Find("Melee");
        if(meleeObject != null)
            melee = meleeObject.GetComponent<Melee>();
    }

    private void Update()
    {
        handleAiming();
        handleShooting();
        handleReloading();
    }

    private void handleAiming()
    {
        Vector3 mousePosition = getMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        // Getting Euler Angle (we want a fixed x and y coordinate system, with the z-axis being the only one rotating for 2D).
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; //Source: https://youtu.be/fuGQFdhSPg4?t=275
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

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
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = getMouseWorldPosition();

            Vector3 gunEndPointPosition = aimGunEndPointTransform.position;
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
    }

    private void handleReloading()
    {
        // Request manual reload if R is pressed.
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(gun != null)
                gun.reload();
        }
    }

    public void setWeaponAnimationStatus(int status)
    {
        animator.SetBool(attackAnimationName, System.Convert.ToBoolean(status));
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
