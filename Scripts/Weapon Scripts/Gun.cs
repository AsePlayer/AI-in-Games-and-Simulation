using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script serves as a general-purpose Gun module that can be added to any Object.
 * They will gain the ability to choose a muzzle flash, request Ammo to shoot, and request Ammo to reload.
 * This is where the bullet spawning occurs.
 */

public class Gun : MonoBehaviour
{
    [SerializeField]
    protected string name;
    [SerializeField]
    protected float reloadTime = 5f;
    [SerializeField]
    protected float weaponCooldown = 0.5f;

    private GameObject ammo;
    public GameObject muzzleflash;

    void Awake()
    {
        // Disable muzzleflash initially until shot is fired.
        muzzleflash = GameObject.Find("Muzzleflash");
        muzzleflash.SetActive(false);

        ammo = GameObject.Find("Ammo");
    }

    public void shoot(Vector3 gunEndPointPosition, float angle, Vector3 aimDirection)
    {
        var ammo = this.ammo.GetComponent<Ammo>();

        // No ammo? No shoot.
        if (ammo == null)
            return;

        // Ammo? Shoot.
        // TODO: make this work with gun cooldown
        if (ammo.shoot())
        {
            muzzleflash.SetActive(true);

            // Spawns bullet where player is facing.
            Bullet bullet = ammo.getBullet();
            GameObject bulleto = Instantiate(bullet.gameObject, gunEndPointPosition, Quaternion.Euler(new Vector3(0, 0, angle)));
            bulleto.GetComponent<Rigidbody2D>().AddForce(aimDirection * bullet.speed);
            bulleto.GetComponent<Bullet>().owner = gameObject;
        }

    }

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
        return ammo.GetComponent<Ammo>();
    }

    public void reload()
    {
        var ammo = this.ammo.GetComponent<Ammo>();
        ammo.startReload();
    }
}
