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
[RAINAction("Detect Allies")]
public class DetectAllies : RAINAction
{
    /// <summary>
    /// The name of the variable that will contain the enemy gameObject when detected.
    /// </summary>
    public Expression AloneVariable = new Expression();

    /// <summary>
    /// We store our TeamElement so we don't have to retrieve it each time we run
    /// </summary>
    private TeamName _teamElement = TeamName.blue;

    private bool alone = false;

    /// <summary>
    /// The enemy variable name evaluated from the EnemyVariable expression
    /// </summary>
    private string _aloneVariableName = null;

    /// <summary>
    /// Set up the teamElement and aloneVariableName
    /// </summary>
    /// <param name="ai">The AI executing the action</param>
    public override void Start(AI ai)
    {
        base.Start(ai);

        _teamElement = ai.Body.GetComponent<IPlayer>().Team;

        _aloneVariableName = null;
        if(AloneVariable.IsValid)
        {
            if (AloneVariable.IsVariable)
            {
                _aloneVariableName = AloneVariable.VariableName;
            }
            else if (AloneVariable.IsConstant)
            {
                _aloneVariableName = AloneVariable.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
            }
        }
    }

    /// <summary>
    /// When executing, this action uses Visual Sensors to look for team aspects. It then
    /// looks for any match, then assign the to the AloneVariable if it could find any ally.
    /// </summary>
    /// <param name="ai">The AI executing the action</param>
    /// <returns>FAILURE if no match was found, SUCCESS otherwise</returns>
    public override ActionResult Execute(AI ai)
    {
        // Fail if our team or variable are missing
        if (string.IsNullOrEmpty(_aloneVariableName))
            return ActionResult.FAILURE;

        // Get all matches from Visual Sensors
        //IList<RAINAspect> matches = ai.Senses.SenseAll();
        IList<RAINAspect> matches = ai.Senses.Sense("Visual Sensor", "team", RAIN.Perception.Sensors.RAINSensor.MatchType.ALL);

        alone = true;
        for (int i = 0; i < matches.Count; i++)
        {
            //Debug.Log(matches[i].AspectType);
            if (matches[i].AspectName == "team")
            {
                TeamAspect aspect = (TeamAspect)matches[i];

                if (aspect == null)
                    continue;

                //Debug.Log(aspect.Entity.EntityName);

                // If we find an aspect that match our team
                if (aspect.team == _teamElement)
                {
                    alone = false;
                    break;
                }
            }
        }

        // set the enemy variable in AI memory
        ai.WorkingMemory.SetItem<bool>(_aloneVariableName, alone);

        // fail if no enemy  found
        if (alone)
            return ActionResult.FAILURE;

        return ActionResult.SUCCESS;
    }
}