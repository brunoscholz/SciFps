using UnityEngine;
using System.Collections;
using System;

public enum GunType { Winchester, Laser }

public interface IGun
{
    GunType Type { get; }
    // Gun Specs
    float MaxClipSize { get; set; }
    float AmmoInCurrentClip { get; set; }
    float ExtraAmmo { get; set; }
    float MaxCarringAmmo { get; set; }

    GameObject Bullet { get; set; }
    GameObject BulletSpawn { get; set; }
    AudioClip BulletSound { get; set; }
    AudioClip ReloadSound { get; set; }
    float FireRate { get; set; }
    float FireTimer { get; set; }

    bool jammed { get; set; }

    void Shot();
}

public class Laser : IGun
{
    // Gun Specs
    float _maxClipSize = 32;
    float _ammoInCurrentClip = 32;
    float _extraAmmo = 128;
    float _maxCarringAmmo = 256;

    GameObject _bullet;
    GameObject _bulletSpawn;
    AudioClip _bulletSound;
    AudioClip _reloadSound;
    float _fireRate;
    float _fireTimer;

    bool _jammed = false;

    public GunType Type
    {
        get { return GunType.Winchester; }
    }

    public float MaxClipSize
    {
        get { return _maxClipSize; }
        set { _maxClipSize = value; }
    }

    public float AmmoInCurrentClip
    {
        get { return _ammoInCurrentClip; }
        set { _ammoInCurrentClip = value; }
    }

    public float ExtraAmmo
    {
        get { return _extraAmmo; }
        set { _extraAmmo = value; }
    }

    public float MaxCarringAmmo
    {
        get { return _maxCarringAmmo; }
        set { _maxCarringAmmo = value; }
    }

    public GameObject Bullet
    {
        get { return _bullet; }
        set { _bullet = value; }
    }

    public GameObject BulletSpawn
    {
        get { return _bulletSpawn; }
        set { _bulletSpawn = value; }
    }

    public AudioClip BulletSound
    {
        get { return _bulletSound; }
        set { _bulletSound = value; }
    }

    public AudioClip ReloadSound
    {
        get { return _reloadSound; }
        set { _reloadSound = value; }
    }

    public float FireRate
    {
        get { return _fireRate; }
        set { _fireRate = value; }
    }

    public float FireTimer
    {
        get { return _fireTimer; }
        set { _fireTimer = value; }
    }

    public bool jammed
    {
        get { return _jammed; }
        set { _jammed = value; }
    }

    public void Shot()
    {
        throw new NotImplementedException();
    }
}

public class Winchester : IGun
{
    // Gun Specs
    float _maxClipSize = 32;
    float _ammoInCurrentClip = 32;
    float _extraAmmo = 128;
    float _maxCarringAmmo = 256;

    GameObject _bullet;
    GameObject _bulletSpawn;
    AudioClip _bulletSound;
    AudioClip _reloadSound;
    float _fireRate;
    float _fireTimer;

    bool _jammed = false;

    public GunType Type
    {
        get { return GunType.Winchester; }
    }

    public float MaxClipSize
    {
        get { return _maxClipSize; }
        set { _maxClipSize = value; }
    }

    public float AmmoInCurrentClip
    {
        get { return _ammoInCurrentClip; }
        set { _ammoInCurrentClip = value; }
    }

    public float ExtraAmmo
    {
        get { return _extraAmmo; }
        set { _extraAmmo = value; }
    }

    public float MaxCarringAmmo
    {
        get { return _maxCarringAmmo; }
        set { _maxCarringAmmo = value; }
    }

    public GameObject Bullet
    {
        get { return _bullet; }
        set { _bullet = value; }
    }

    public GameObject BulletSpawn
    {
        get { return _bulletSpawn; }
        set { _bulletSpawn = value; }
    }

    public AudioClip BulletSound
    {
        get { return _bulletSound; }
        set { _bulletSound = value; }
    }

    public AudioClip ReloadSound
    {
        get { return _reloadSound; }
        set { _reloadSound = value; }
    }

    public float FireRate
    {
        get { return _fireRate; }
        set { _fireRate = value; }
    }

    public float FireTimer
    {
        get { return _fireTimer; }
        set { _fireTimer = value; }
    }

    public bool jammed
    {
        get { return _jammed; }
        set { _jammed = value; }
    }

    public void Shot()
    {
        throw new NotImplementedException();
    }
}
