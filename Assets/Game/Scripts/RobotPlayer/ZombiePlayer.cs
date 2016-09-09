using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Entities;

[RequireComponent(typeof(AudioSource))]
public class ZombiePlayer : MonoBehaviour, IPlayer
{
    Animator animator;
    AI ai;
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
        shooter = GetComponent<IShooter>();
        animator = GetComponent<Animator>();
        ai = GetComponentInChildren<AIRig>().AI;
        eRig = GetComponentInChildren<EntityRig>().Entity;
    }

    void Start()
    {
        SetUpPlayer(Team);
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

    }

	void Update ()
    {
        if (_health < 0)
        {
            _health = 0;
            StartCoroutine(Die());
        }
    }

    public bool Attack(IPlayer target)
    {
        // If we can't attack we're already done
        if (!CanAttack || !target.CanTakeDamage)
            return false;

        // Figure out how much we will hurt them
        Hit shot = new Hit(this, "normal", false);
        float tDamage = _damage - Random.Range(0, Mathf.Clamp01(1 - _accuracy)) * _damage;
        shot.SetDamage(tDamage);

        // Shoot
        LaserBullet bt = shooter.Shoot();
        if(bt)
        {
            bt.Shot(shot);

            // Do damage
            target.TakeDamage(shot);
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
        //animator.SetTrigger("Hit");
        ai.WorkingMemory.SetItem("HIT", true);
        _health -= hit.Damage - armor;

        if (HitParticle)
            Instantiate(HitParticle, hit.Direction.point + (hit.Direction.normal * hitParticleSpacing), Quaternion.LookRotation(hit.Direction.normal));

        if (HitSound)
            AudioSource.PlayClipAtPoint(HitSound, hit.Direction.point);
    }

    public IEnumerator Die()
    {
        ai.WorkingMemory.SetItem("dead", true);
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
