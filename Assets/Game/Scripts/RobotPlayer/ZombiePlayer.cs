using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Entities;
using System;

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

    public Transform Body
    {
        get { return transform; }
    }

    int myID;
    public int teamID
    {
        get { return myID; }
        set { myID = value; }
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
        set
        {
            _health = Mathf.Min(100, value);
        }
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

    public void SetUpPlayer(TeamName t, int id)
    {
        Team = t;
        myID = id;

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
        rend.material.SetColor("_EmissionColor", TeamSets.Colors[Team.ToString()]);
    }

    public void SetColors(Hud h) { }

    public void Respawn()
    {
        _health = 100;
        animator.SetBool("Dead", false);
        SetAction("dead", false);
        SetAction("enemyPosition", new Vector3());
        SetCommand("patrol");
        StartCoroutine(Reactivate());
    }

    IEnumerator Reactivate()
    {
        eRig.ActivateEntity();
        yield return new WaitForSeconds(0.5f);
        SetAction("start", true);
    }

    public void SetCommander(GameObject go)
    {
        SetAction("myCommander", go);
    }

    public void SetCommand(string action)
    {
        SetAction("command", action);
    }

    public void SetAction(string name, string value)
    {
        ai.WorkingMemory.SetItem(name, value);
    }

    public void SetAction(string name, float value)
    {
        ai.WorkingMemory.SetItem(name, value);
    }

    public void SetAction(string name, bool value)
    {
        ai.WorkingMemory.SetItem(name, value);
    }

    public void SetAction(string name, Vector3 value)
    {
        ai.WorkingMemory.SetItem(name, value);
    }

    public void SetAction(string name, GameObject value)
    {
        ai.WorkingMemory.SetItem(name, value);
    }

    void Update ()
    {
        if (_health < 0)
            _health = 0;
    }

    public bool Attack(IPlayer target)
    {
        // If we can't attack we're already done
        if (!CanAttack || !target.CanTakeDamage)
            return false;

        // Figure out how much we will hurt them
        Hit shot = new Hit(this, "normal", false);
        float tDamage = _damage - UnityEngine.Random.Range(0, Mathf.Clamp01(1 - _accuracy)) * _damage;
        shot.SetDamage(tDamage);

        // Shoot
        LaserBullet bt = shooter.Shoot();
        if(bt && target.Team != Team)
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

        _health = Mathf.Max(0, _health - hit.Damage);

        if (!IsAlive)
        {
            _invulnerableTimer = Time.time + 4;
            StartCoroutine(Die());
        }

        return hit.Damage > 0;
    }

    public void Hit(Hit hit)
    {
        float armor = UnityEngine.Random.Range(2, 10);
        //animator.SetTrigger("Hit");
        SetAction("HIT", true);
        _health -= hit.Damage - armor;

        if (HitParticle)
            Instantiate(HitParticle, hit.Direction.point + (hit.Direction.normal * hitParticleSpacing), Quaternion.LookRotation(hit.Direction.normal));

        if (HitSound)
            AudioSource.PlayClipAtPoint(HitSound, hit.Direction.point);
    }

    public IEnumerator Die()
    {
        SetAction("dead", true);
        eRig.DeactivateEntity();
        animator.SetFloat("Speed", 0);
        GameManager manager = GameObject.FindObjectOfType<GameManager>();
        manager.ReportDown(this);

        yield return new WaitForSeconds(5);
        SetAction("start", false);
        manager.Respawn(this);
    }

    public void Eliminate()
    {
        Destroy(gameObject);
    }
}
