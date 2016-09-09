using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Text totalBullets;
    public Text currentBullets;
    public Text notification;

    public IEnumerator ShowNotification(string txt)
    {
        notification.text = txt;
        notification.enabled = true;
        yield return new WaitForSeconds(3f);
        notification.enabled = false;
    }
}
