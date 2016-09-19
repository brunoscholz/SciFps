using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Text totalBullets;
    public Text currentBullets;
    public Text notification;

    public Text redScore;
    public Text blueScore;
    public Text redFlags;
    public Text blueFlags;

    public Text timer;

    public Image crosshair;
    public GameObject weapon;

    public Slider healthSlider;
    public Text healthText;

    public IEnumerator ShowNotification(string txt)
    {
        notification.text = txt;
        notification.enabled = true;
        yield return new WaitForSeconds(3f);
        notification.enabled = false;
    }
}
