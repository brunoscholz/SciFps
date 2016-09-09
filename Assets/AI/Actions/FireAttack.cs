using RAIN.Action;
using RAIN.Core;
using RAIN.Entities;
using RAIN.Entities.Aspects;
using RAIN.Representation;
using RAIN.Motion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FireAttack is a RAIN behavior tree action that looks shots at enemies.
/// </summary>
[RAINAction("Fire Attack")]
public class FireAttack : RAINAction
{
    IPlayer _player;
    IPlayer _myEnemy;

    public override void Start(AI ai)
    {
        base.Start(ai);
        _player = ai.Body.GetComponent<IPlayer>();
        _myEnemy = ai.WorkingMemory.GetItem<GameObject>("enemy").GetComponent<IPlayer>();
    }

    public override ActionResult Execute(AI ai)
    {
        if(_player == null)
            _player = ai.Body.GetComponent<IPlayer>();

        if (_myEnemy == null)
            _myEnemy = ai.WorkingMemory.GetItem<GameObject>("enemy").GetComponent<IPlayer>();

        _player.Attack(_myEnemy);
        return ActionResult.SUCCESS;
    }
}
