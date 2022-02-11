using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script serves as a general-purpose Melee module that can be added to any Object.
 * They will gain the ability to choose a slice effect and swing hitbox
 * This is where the swing spawning occurs.
 */
public class Melee : MonoBehaviour
{
    [SerializeField] GameObject owner;
    [SerializeField] PlayerAimWeapon playerAimWeapon;
    [SerializeField] protected string name;
    [SerializeField] protected float weaponCooldown = 0.15f;
    [SerializeField] protected bool weaponOnCooldown;
    [SerializeField] protected GameObject swish;
    [SerializeField] protected GameObject hitbox;
    [SerializeField] protected int damage;
    [SerializeField] protected bool spawnProjectile;
    

    void Awake()
    {
        // Disable weapon slash initially until hit is triggered.
        swish.SetActive(false);
        // Set animator to Parent Object's Animator Component.
        playerAimWeapon = gameObject.transform.root.GetComponent<PlayerAimWeapon>();
        playerAimWeapon.setAnimation("isAttackingMelee");
    }


    public void swing(Vector3 gunEndPointPosition, float angle, Vector3 aimDirection)
    {
        // On weapon cooldown? No Swing.
        if (weaponOnCooldown)
            return;

        //swish.SetActive(true);  make this better first
        
        playerAimWeapon.setWeaponAnimationStatus(1);


        StartCoroutine(waitWeaponSwing(gunEndPointPosition, angle, aimDirection));

        // Weapon cooldown in between shots
        StartCoroutine(waitWeaponCooldown());
    }


    /*
     * Getters
     */


    public float getWeaponCooldown()
    {
        return weaponCooldown;
    }

    public void setSpawnProjectile(bool info)
    {
        spawnProjectile = info;
    }


    /*
     * Courotines
     */

    public IEnumerator waitWeaponSwing(Vector3 gunEndPointPosition, float angle, Vector3 aimDirection)
    {
        // Don't cause infinite stall.
        if (spawnProjectile)
            yield break;

        yield return new WaitUntil(() => spawnProjectile == true);
        // Treating a melee attack as a stationary bullet.
        GameObject hitbox = Instantiate(this.hitbox.gameObject, playerAimWeapon.aimGunEndPointTransform.position, Quaternion.Euler(new Vector3(0, 0, playerAimWeapon.angle)));
        
        // Cache this info soon
        hitbox.GetComponent<Swing>().damage = this.damage;
        hitbox.GetComponent<Swing>().owner = gameObject.transform.root.gameObject;
        spawnProjectile = false;
    }
    public IEnumerator waitWeaponCooldown()
    {
        weaponOnCooldown = true;
        yield return new WaitForSeconds(weaponCooldown);
        weaponOnCooldown = false;
    }

}
