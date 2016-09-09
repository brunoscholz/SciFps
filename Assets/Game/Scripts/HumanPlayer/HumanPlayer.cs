using UnityEngine;
using System.Collections;
using RAIN.Entities;

[RequireComponent(typeof(AudioSource))]
public class HumanPlayer : MonoBehaviour, IPlayer
{
    Hud hud;
    Animator animator;
    Entity eRig;
    IShooter shooter;

    public TeamName team = TeamName.blue;
    public TeamName Team
    {
        get { return team; }
        set { team = value; }
    }

    [SerializeField]
    private float _health = 100;

    [SerializeField]
    private float _damage = 10;

    [SerializeField]
    private float _accuracy = 0.5f;

    [SerializeField]
    private float _incapacitationTime = 1;

    [SerializeField]
    private float _invulnerabilityTime = 1;

    private float _incapacitatedTimer = float.MinValue;
    private float _invulnerableTimer = float.MinValue;

    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public float Accuracy
    {
        get { return _accuracy; }
        set { _accuracy = value; }
    }

    public bool CanAttack
    {
        get { return (Time.time > _incapacitatedTimer); }
    }

    public bool CanTakeDamage
    {
        get { return (Time.time > _invulnerableTimer); }
    }

    public bool IsAlive
    {
        get { return (_health > 0); }
    }

    float hitParticleSpacing = 0.001f;
    public GameObject HitParticle;
    public AudioClip HitSound;

    public GunType GunType = GunType.Laser;

    void Awake()
    {
        hud = GetComponent<Hud>();
        shooter = GetComponent<IShooter>();
        animator = GetComponent<Animator>();
        eRig = GetComponentInChildren<EntityRig>().Entity;
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Reload"))
        {
            shooter.Reload();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Attack(null);
        }

        hud.totalBullets.text = shooter.Gun.ExtraAmmo.ToString();
        hud.currentBullets.text = shooter.Gun.AmmoInCurrentClip.ToString();
    }

    public void DidHeadshot()
    {
        StartCoroutine(hud.ShowNotification("HEADSHOT!!!"));
    }

    public void SetUpPlayer(TeamName t)
    {
        Team = t;
        TeamAspect ta = new TeamAspect(Team);
        eRig.AddAspect(ta);

        SetColors();
        shooter.GunType = GunType;
    }

    public void SetColors()
    {
        //transform.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("albedo", TeamSets.Colors[Team.ToString()]);
        SkinnedMeshRenderer rend = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        //rend.material.shader = Shader.Find("Albedo");
        rend.material.SetColor("_Color", TeamSets.Colors[Team.ToString()]);
    }

    public bool Attack(IPlayer target)
    {
        // Figure out how much we will hurt them
        Hit shot = new Hit(this, "normal", false);
        float tDamage = _damage - Random.Range(0, Mathf.Clamp01(1 - _accuracy)) * _damage;
        shot.SetDamage(tDamage);

        LaserBullet bt = shooter.Shoot();
        if(bt)
        {
            bt.Shot(shot, true);

        }

        return tDamage > 0;

    }

    public bool TakeDamage(Hit hit)
    {
        if (!CanTakeDamage)
            return false;

        // A very simple health
        _health = Mathf.Max(0, _health - hit.Damage);

        // Getting damage makes us invulnerable, but we can't attack either
        if (hit.Damage > 0)
        {
            _incapacitatedTimer = Time.time + _incapacitationTime;
            _invulnerableTimer = Time.time + _invulnerabilityTime;
        }

        return hit.Damage > 0;
    }

    public void Hit(Hit hit)
    {
        float armor = Random.Range(2, 10);
        
        // Animation for HIT
        //animator.SetTrigger("Hit");
        
        _health -= hit.Damage - armor;

        if (HitParticle)
            Instantiate(HitParticle, hit.Direction.point + (hit.Direction.normal * hitParticleSpacing), Quaternion.LookRotation(hit.Direction.normal));

        if (HitSound)
            AudioSource.PlayClipAtPoint(HitSound, hit.Direction.point);
    }

    public IEnumerator Die()
    {
        animator.SetFloat("Speed", 0);

        animator.SetBool("Dead", true);
        yield return null;
        animator.SetBool("Dead", false);

        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = false;
        }

        Debug.Log("DIE MOTHERFUCKER!!!!");
        Destroy(gameObject, 15f);
    }
}
