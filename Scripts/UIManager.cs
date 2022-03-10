using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    Score score;
    Ammo ammo;
    AimWeapon aw;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI ammoText;
    // Start is called before the first frame update
    void Start()
    {
        score = player.GetComponent<Score>();
        aw = player.GetComponent<AimWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.getScoreTotal()/*.ToString()*/;
        // If player has gun, display ammo information
        if(aw != null && aw.gun != null)
        {
            ammo = aw.gun.getAmmo();
            ammoText.text = ammo.getAmmoInMag().ToString() + "/" + ammo.getAmmoCapacity().ToString();
        }
        // If player has melee, display 0/0
        else
        {
            ammoText.text = "0/0";
        }
    }


}
