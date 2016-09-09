using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public IPlayer owner;
    public GameObject HitWallParticle;

    float range = 10000;
    bool canshot = false;

    void Update()
    {
        if (!canshot)
            return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if(hit.transform.tag == "Enemy")
            {
                Debug.Log("<color=blue>Normal Hit to an enemy!</color>");
                Hit shot = new Hit(owner, "normal", false, hit);
                shot.SetDamage(30);
                hit.transform.GetComponent<IPlayer>().Hit(shot);
            }
            else if(hit.transform.tag == "EnemyHead")
            {
                Debug.Log("<color=red>HEADSHOT!!!</color>");
                Hit shot = new Hit(owner, "headshot", false, hit);
                shot.SetDamage(80);
                hit.transform.GetComponentInParent<IPlayer>().Hit(shot);
                //owner.DidHeadshot();
            }
            else
            {
                if (HitWallParticle)
                {
                    Instantiate(HitWallParticle, hit.point + (hit.normal * 0.001f), Quaternion.LookRotation(hit.normal));
                }
            }
        }

        Destroy(gameObject);
    }

    public void Shot()
    {
        canshot = true;
    }
}
