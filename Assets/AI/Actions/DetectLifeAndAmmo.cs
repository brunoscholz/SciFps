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
/// DetectEnemy is a RAIN behavior tree action that looks for enemies based on their Team.
/// To use it, create a custom action in your behavior tree and choose the "Detect Enemy" action.
/// Set EnemyVariable to the name of the variable you want to assign the enemy to in AI memory.
/// </summary>
[RAINAction("Detect Life And Ammo")]
public class DetectLifeAndAmmo : RAINAction
{
    /// <summary>
    /// We store our IPlayer so we don't have to retrieve it each time we run
    /// </summary>
    private IPlayer _playerElement = null;

    private float _myhealth = -1f;
    private float _myammo = -1f;

    public Expression HealthLimit = new Expression();
    private float _healthLimit = 0;


    /// <summary>
    /// Set up the teamElement and enemyVariableName
    /// </summary>
    /// <param name="ai">The AI executing the action</param>
    public override void Start(AI ai)
    {
        base.Start(ai);

        // only look for the teamElement if we don't already have one
        //if (_teamElement == null)
        //    _teamElement = ai.GetCustomElement<TeamElement>();

        _playerElement = ai.Body.GetComponent<IPlayer>();

        _healthLimit = 0;
        if (HealthLimit.IsValid)
        {
            _healthLimit = HealthLimit.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
        }
    }

    /// <summary>
    /// When executing, this action uses Visual Sensors to look for team aspects. It then
    /// looks for either the nearest match or a known enemy, then assign the value (or null)
    /// to the EnemyVariable.
    /// </summary>
    /// <param name="ai">The AI executing the action</param>
    /// <returns>FAILURE if no match was found, SUCCESS otherwise</returns>
    public override ActionResult Execute(AI ai)
    {
        // Fail if our team or variable are missing
        if (_healthLimit <= 0)
            return ActionResult.FAILURE;
        if (_playerElement == null)
            return ActionResult.FAILURE;

        if(_myhealth < _healthLimit)
        {
            // set the enemy variable in AI memory
            ai.WorkingMemory.SetItem("command", "heal");
            return ActionResult.SUCCESS;
        }


        return ActionResult.FAILURE;
    }
}