using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script serves as a general-purpose Ammo module that can be added to any Object.
 * They will gain the ability to track ammo, trigger shot for gun, and reload.
 */

public class Ammo : MonoBehaviour
{
    [SerializeField] protected Gun gun;
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected int ammoCapacity = 60;
    [SerializeField] protected int magCapacity = 10;
    [SerializeField] protected int ammoInMag;
    [SerializeField] protected bool isReloading;
    AimWeapon aimWeapon;

    void Awake()
    {
        gun = this.GetComponent<Gun>();
        ammoInMag = magCapacity;
        aimWeapon = gameObject.transform.root.GetComponent<AimWeapon>();
    }

    public bool shotPossible()
    {
        // No bullet? No shoot.
        if (bullet == null)
            return false;

        // No ammo? No shoot.
        if (ammoInMag <= 0 && ammoCapacity <= 0)
            return false;

        // Reloading? No shoot.
        if (isReloading)
            return false;

        // No ammo but still got ammoCapacity? Reload.
        if (ammoInMag <= 0)
        {
            startReload();
            return false;
        }

        // All conditions to shoot are met, subtract ammo.
        ammoInMag--;

        // Shot last bullet? Call a reload
        if (ammoInMag <= 0)
            startReload();

        // Passed all the guards. Return true for shot possible.
        return true;
    }

    public void startReload()
    {
        // No ammo left? No reload.
        if (ammoCapacity <= 0)
            return;

        // Full ammo? No reload.
        if (ammoInMag == magCapacity)
            return;

        aimWeapon.setWeaponAnimationStatus("isReloading", 1);
        StartCoroutine(reload());
    }

    public Bullet getBullet()
    {
        return bullet;
    }

    IEnumerator reload()
    {
        // Start reloading
        isReloading = true;

        
        // Rollover excess reload ammo because we're nice :)
        int ammoToTakeFromCapacity = magCapacity - ammoInMag;

        bool fullReload;
        // If you need to reload more than you have left, reload everything you have left. Mark fullReload as false.
        // Otherwise, execute a full reload.
        if (ammoCapacity < ammoToTakeFromCapacity)
        {
            ammoToTakeFromCapacity = ammoCapacity;
            fullReload = false;
        }
        else
        {
            fullReload = true;
        }
        
        // Wait to dispense ammo until reload time has elapsed.
        yield return new WaitForSeconds(gun.getReloadTime());

        // Handle reload according to ammo count.
        if (fullReload)
            ammoInMag = magCapacity;
        else
            ammoInMag = ammoToTakeFromCapacity + ammoInMag;

        // Subtract ammo
        ammoCapacity -= ammoToTakeFromCapacity;

        // Reloading is complete
        isReloading = false;
        aimWeapon.setWeaponAnimationStatus("isReloading", 0);
    }

}
