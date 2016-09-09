using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Hit
{
    private float mDamage;

    public string Attack;
    public IPlayer Attacker;
    public bool CanDodge;
    public RaycastHit Direction;

    public float Damage
    {
        get
        {
            return mDamage;
        }
    }

    public Hit(IPlayer attacker, string attack, bool canDodge, RaycastHit direction)
    {
        Attacker = attacker;
        Attack = attack;
        CanDodge = canDodge;
        Direction = direction;
    }

    public Hit(IPlayer attacker, string attack, bool canDodge)
    {
        Attacker = attacker;
        Attack = attack;
        CanDodge = canDodge;
    }

    public void CreateEffect(Vector3 position, GameObject go)
    {
        //Effect newFx = new Effect(position, Direction, Attack.EffectType, Attack.Element);
        //return newFx;
    }

    public void SetDamage(float damage)
    {
        mDamage = damage;
    }

    public void SetDirection(RaycastHit direction)
    {
        Direction = direction;
    }
}
