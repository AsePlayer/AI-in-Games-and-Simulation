using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    Bullet bullet;

    [SerializeField]
    int ammoCapacity = 60;
    [SerializeField]
    int magCapacity = 10;
    [SerializeField]
    int ammoInMag;

    GameObject gun;
    bool isReloading;
    // Start is called before the first frame update
    void Awake()
    {
        ammoInMag = magCapacity;
        gun = GameObject.Find("Gun");

    }

    public bool shoot()
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

        return true;
    }

    public void startReload()
    {
        if (ammoCapacity <= 0)
            return;

        if (ammoInMag == magCapacity)
            return;
           
        StartCoroutine(reload());
        
    }

    public Bullet getBullet()
    {
        return bullet;
    }

    IEnumerator reload()
    {
        isReloading = true;
        
        // Rollover excess reload ammo because we're nice :)
        int ammoToTakeFromCapacity = magCapacity - ammoInMag;

        // If you need to reload more than you have left, reload everything you have left.
        bool fullReload;

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
        yield return new WaitForSeconds(gun.GetComponent<Gun>().getReloadTime());

        // Handle reload according to ammo count.
        if (fullReload)
            ammoInMag = magCapacity;
        else
            ammoInMag = ammoToTakeFromCapacity + ammoInMag;

        // Subtract ammo
        ammoCapacity -= ammoToTakeFromCapacity;

        isReloading = false; ;
    }
}
