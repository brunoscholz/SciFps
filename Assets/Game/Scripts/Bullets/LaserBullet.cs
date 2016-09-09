using UnityEngine;
using System.Collections;

public class LaserBullet : MonoBehaviour
{
    [HideInInspector]
    public IPlayer owner;
    public GameObject HitWallParticle;

    public Transform shotPoint;
    public TeamName Team = TeamName.blue;

    float range = 1000;

    RaycastHit hit;
    Ray ray;

    void Awake()
    {
        GetComponent<ParticleSystem>().startColor = TeamColors.Colors[Team.ToString()];
    }

    void Update()
    {
        
    }

    public void Shot(Hit shot, bool human = false)
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, range))
        {
            StartCoroutine(FireLaser());

            if (hit.transform.tag == "Enemy")
            {
                Debug.Log("<color=blue>Normal Hit to an enemy!</color>");
                //Hit shot = new Hit(owner, "normal", false, hit);
                //shot.SetDamage(30);
                hit.transform.GetComponent<IPlayer>().Hit(shot);
            }
            else if (hit.transform.tag == "EnemyHead")
            {
                Debug.Log("<color=red>HEADSHOT!!!</color>");
                //Hit shot = new Hit(owner, "headshot", false, hit);
                shot.SetDamage(shot.Damage * 7.5f);
                hit.transform.GetComponentInParent<IPlayer>().Hit(shot);
                //owner.DidHeadshot();
            }
            else
            {
                if (HitWallParticle)
                {
                    Instantiate(HitWallParticle, hit.point + (hit.normal * 0.2f), Quaternion.FromToRotation(Vector3.up, hit.normal)); //Quaternion.LookRotation(hit.normal)
                }
            }
        }
        else
            StartCoroutine(FireLaser(true));
    }

    IEnumerator FireLaser(bool noHit = false)
    {
        //float animTime = 0.7f;
        //Vector3 origin = ray.origin;
        //Vector3 target = noHit ? ray.GetPoint(30) : hit.point;
        //Vector3 direction = (target - ray.origin).normalized;

        //line.enabled = true;

        //while (animTime > 0)
        //{
        //    animTime -= Time.deltaTime;
        //    origin += (direction * Time.deltaTime * 50);
        //    line.SetPosition(0, origin);
        //    line.SetPosition(1, target);

        //    yield return null;
        //}

        //line.enabled = false;
        yield return new WaitForSeconds(0.45f);
        Destroy(gameObject);
    }
}
