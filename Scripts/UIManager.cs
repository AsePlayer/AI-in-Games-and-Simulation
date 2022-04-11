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
    [SerializeField] private TextMeshProUGUI swapText;
    [SerializeField] private Image swapPanel;
    [SerializeField] private Image howToPlayPanel;
    [SerializeField] private Image howToPlayBackgroundPanel;

    public bool nearWeapon;
    // Start is called before the first frame update
    void Start()
    {
        score = player.GetComponent<Score>();
        aw = player.GetComponent<AimWeapon>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.timeScale == 0)
        {
            howToPlayPanel.enabled = true;
            howToPlayBackgroundPanel.enabled = true;
            
            scoreText.text = "";
            ammoText.text = "";
            swapText.enabled = false;
            swapPanel.enabled = false;
        }
        else
        {
            howToPlayPanel.enabled = false;
            howToPlayBackgroundPanel.enabled = false;
        
            scoreText.text = "Enemies Remaining: " + score.getScoreTotal()/*.ToString()*/;
            // If player has gun, display ammo information

            if (aw != null && aw.gun != null)
            {
                ammo = aw.gun.getAmmo();
                ammoText.text = "Ammo: " + ammo.getAmmoInMag().ToString() + "/" + ammo.getAmmoCapacity().ToString();
            }
            // If player has melee, display 0/0
            else
            {
                ammoText.text = "Ammo: 0/0";
            }

            if (score.getScoreTotal() <= 0)
            {
                // New level
            }


            Gun[] allGuns = GameObject.FindObjectsOfType<Gun>();
            Melee[] allMelees = GameObject.FindObjectsOfType<Melee>();

            swapText.enabled = false;
            swapPanel.enabled = false;

            foreach (Gun currentGun in allGuns)
            {
                if (currentGun.gameObject.GetComponent<SpriteRenderer>().color == Color.cyan)
                {
                    swapText.enabled = true;
                    swapPanel.enabled = true;
                    break;
                }

            }

            foreach (Melee currentMelee in allMelees)
            {
                if (currentMelee.gameObject.GetComponent<SpriteRenderer>().color == Color.cyan)
                {
                    swapText.enabled = true;
                    swapPanel.enabled = true;
                    break;
                }

            }
        }
    }
}
