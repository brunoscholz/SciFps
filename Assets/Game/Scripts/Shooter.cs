using UnityEngine;
using System.Collections;
using RAIN.Entities;

public class Shooter : MonoBehaviour, IShooter
{
    public GameObject _bullet;
    public GameObject _bulletSpawn;
    public AudioClip _bulletSound;
    public AudioClip _reloadSound;
    public float _fireRate;
    public float _fireTimer;

    bool _isAiming = false;
    public bool IsAiming
    {
        get { return _isAiming; }
        set { _isAiming = value; }
    }

    public GunType GunType
    {
        get { return myGun.Type; }
        set
        {
            switch (value)
            {
                case GunType.Laser:
                    SetNewGun(new Laser());
                    break;
                case GunType.Winchester:
                    SetNewGun(new Winchester());
                    break;
                default:
                    break;
            }
        }
    }

    IGun myGun = new Winchester();
    public IGun Gun
    {
       get { return myGun; }
    }

    bool Reloading = false;
    float reloadTimer = 2.63f;
    float reloatCountTimer;

    void SetNewGun(IGun g)
    {
        myGun = g;
        myGun.Bullet = _bullet;
        myGun.BulletSpawn = _bulletSpawn;
        myGun.BulletSound = _bulletSound;
        myGun.ReloadSound = _reloadSound;
        myGun.FireRate = _fireRate;
        myGun.FireTimer = _fireTimer;
}

    void Update()
    {
        AimingInController();
    }

    void AimingInController()
    {
        IsAiming = false;
    }

    void LateUpdate()
    {
        if (myGun.AmmoInCurrentClip > myGun.MaxClipSize)
            myGun.AmmoInCurrentClip = myGun.MaxClipSize;

        if (myGun.ExtraAmmo > myGun.MaxCarringAmmo)
            myGun.ExtraAmmo = myGun.MaxCarringAmmo;

        if (myGun.FireTimer < -5)
            myGun.FireTimer = -5;

        if (myGun.MaxClipSize < 0)
            myGun.MaxClipSize = 0;

        if (myGun.AmmoInCurrentClip < 0)
            myGun.AmmoInCurrentClip = 0;

        //totalBullets.text = ExtraAmmo.ToString();
        //currentBullets.text = AmmoInCurrentClip.ToString();

        if (Reloading) //!ReloadAnimation.animation.IsPlaying(ReloadName)
        {
            if (reloatCountTimer <= 0)
            {
                if (myGun.ExtraAmmo >= myGun.MaxClipSize - myGun.AmmoInCurrentClip)
                {
                    myGun.ExtraAmmo -= myGun.MaxClipSize - myGun.AmmoInCurrentClip;
                    myGun.AmmoInCurrentClip = myGun.MaxClipSize;
                }

                if (myGun.ExtraAmmo < myGun.MaxClipSize - myGun.AmmoInCurrentClip)
                {
                    myGun.AmmoInCurrentClip += myGun.ExtraAmmo;
                    myGun.ExtraAmmo = 0;
                }
                Reloading = false;
            }
            else
            {
                reloatCountTimer -= Time.deltaTime;
            }
        }

        myGun.FireTimer -= Time.deltaTime * myGun.FireRate;
    }

    public void Reload()
    {
        if (!Reloading && myGun.AmmoInCurrentClip < myGun.MaxClipSize && myGun.ExtraAmmo > 0 && IsAiming == false)
        {
            Reloading = true;
            reloatCountTimer = reloadTimer;
            myGun.BulletSpawn.GetComponent<AudioSource>().PlayOneShot(myGun.ReloadSound);
        }
    }

    public LaserBullet Shoot()
    {
        LaserBullet bt = null;

        if (!Reloading && myGun.AmmoInCurrentClip == 0 && myGun.ExtraAmmo > 0 && IsAiming == false)
        {
            Reloading = true;
            reloatCountTimer = reloadTimer;
            myGun.BulletSpawn.GetComponent<AudioSource>().PlayOneShot(myGun.ReloadSound);
        }
        
        if (myGun.AmmoInCurrentClip > 0 && !Reloading)
        {
            if (myGun.FireTimer <= 0)
            {
                myGun.AmmoInCurrentClip -= 1;

                if (myGun.Bullet)
                {
                    bt = (Instantiate(myGun.Bullet, myGun.BulletSpawn.transform.position, myGun.BulletSpawn.transform.rotation) as GameObject).GetComponent<LaserBullet>();
                    //bt.owner = this;
                    bt.shotPoint = myGun.BulletSpawn.transform;
                }

                if (myGun.BulletSound)
                    myGun.BulletSpawn.GetComponent<AudioSource>().PlayOneShot(myGun.BulletSound);

                myGun.FireTimer = 1;
            }
        }

        return bt;
    }
}
