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
[RAINAction("Ask for Base Position")]
public class AskForBasePosition : RAINAction
{
    /// <summary>
    /// We store our IPlayer so we don't have to retrieve it each time we run
    /// </summary>
    private IPlayer _playerElement = null;

    public Expression BasePositionVariable = new Expression();
    private string _basePositionVariableName = null;

    GameObject myCommander = null;

    /// <summary>
    /// Set up the teamElement and enemyVariableName
    /// </summary>
    /// <param name="ai">The AI executing the action</param>
    public override void Start(AI ai)
    {
        base.Start(ai);

        _playerElement = ai.Body.GetComponent<IPlayer>();

        myCommander = ai.WorkingMemory.GetItem<GameObject>("myCommander");

        _basePositionVariableName = null;
        if (BasePositionVariable.IsValid)
        {
            if (BasePositionVariable.IsVariable)
            {
                _basePositionVariableName = BasePositionVariable.VariableName;
            }
            else if (BasePositionVariable.IsConstant)
            {
                _basePositionVariableName = BasePositionVariable.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
            }
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
        if (string.IsNullOrEmpty(_basePositionVariableName))
            return ActionResult.FAILURE;
        if (_playerElement == null)
            return ActionResult.FAILURE;
        if (myCommander == null)
            return ActionResult.FAILURE;

        Vector3 _basePosition = myCommander.GetComponent<GameManager>().GetTeamBase(_playerElement.Team);

        return ActionResult.SUCCESS;
    }
}