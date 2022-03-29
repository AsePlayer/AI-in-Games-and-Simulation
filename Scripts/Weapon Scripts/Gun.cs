using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script serves as a general-purpose Gun module that can be added to any Object.
 * They will gain the ability to choose a muzzle flash, request Ammo to shoot, and request Ammo to reload.
 * This is where the bullet spawning occurs.
 * 
 * REQUIRES Ammo Component!
 */

[RequireComponent(typeof(Ammo))]
public class Gun : MonoBehaviour
{
    [SerializeField] protected string name;
    [SerializeField] protected float reloadTime = 5f;
    [SerializeField] protected float weaponCooldown = 0.5f;
    [SerializeField] public bool weaponOnCooldown;
    [SerializeField] protected GameObject muzzleflash;
    [SerializeField] public string reloadAnimation;
    [SerializeField] protected int damage;

    private Ammo ammo;
    private Bullet bullet;

    void Awake()
    {
        // Disable muzzleflash initially until shot is fired.
        muzzleflash.SetActive(false);

        // Cache information that will be accessed often.
        ammo = this.GetComponent<Ammo>();
        bullet = ammo.getBullet();
    }


    public void reload()
    {
        ammo.startReload();
    }


    public void shoot(Vector3 gunEndPointPosition, float angle, Vector3 aimDirection)
    {
        // No ammo? No shoot.
        if (ammo == null)
        {
            return;
        }

        // On weapon cooldown? No Shoot.
        if (weaponOnCooldown)
        {
            return;
        }

        // Ammo? Shoot.
        if (ammo.shotPossible())
        {
            muzzleflash.SetActive(true);

            // Spawns bullet where player is facing.
            Bullet b = ammo.getBullet();
            b.damage = damage;
            b.owner = gameObject.transform.root.gameObject;

            GameObject bullet = Instantiate(b.gameObject, gunEndPointPosition, Quaternion.Euler(new Vector3(0, 0, angle)));
            bullet.GetComponent<Rigidbody2D>().AddForce(aimDirection * this.bullet.speed);

            // Weapon cooldown in between shots
            StartCoroutine(waitWeaponCooldown());
        }

    }


    /*
     * Getters
     */

    public float getReloadTime()
    {
        return reloadTime;
    }

    public float getWeaponCooldown()
    {
        return weaponCooldown;
    }
    
    public Ammo getAmmo()
    {
        return ammo;
    }

    /*
     * Courotines
     */

    public IEnumerator waitWeaponCooldown()
    {
        weaponOnCooldown = true;
        yield return new WaitForSeconds(weaponCooldown);
        weaponOnCooldown = false;
    }
}
