using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    protected string name;
    [SerializeField]
    protected float reloadTime = 5f;
    [SerializeField]
    protected float weaponCooldown = 0.5f;

    public GameObject muzzleflash;

    private GameObject ammo;


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

        if (ammo.shoot())
        {
            // Spawns bullet where player is facing.
            Bullet bullet = ammo.getBullet();
            muzzleflash.SetActive(true);
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
