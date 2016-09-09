using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Navigation;
using RAIN.Representation;

[RAINAction("Update Attack Position")]
public class AIUpdateAttackPosition : RAINAction
{
    /// <summary>
    /// The name of the variable that will contain the enemy gameObject when detected.
    /// </summary>
    //public Expression EnemyVariable = new Expression();
    public Expression AttackDistance = new Expression();
    public Expression MoveVariable = new Expression();

    /// <summary>
    /// The enemy variable name evaluated from the EnemyVariable expression
    /// </summary>
    private string _enemyVariableName = null;
    private string _moveVariableName = null;

    public override void Start(AI ai)
    {
        base.Start(ai);

        //_enemyVariableName = null;
        //if (EnemyVariable.IsValid)
        //{
        //    if (EnemyVariable.IsVariable)
        //    {
        //        _enemyVariableName = EnemyVariable.VariableName;
        //    }
        //    else if (EnemyVariable.IsConstant)
        //    {
        //        _enemyVariableName = EnemyVariable.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
        //    }
        //}

        _moveVariableName = null;
        if (MoveVariable.IsValid)
        {
            if (MoveVariable.IsVariable)
            {
                _moveVariableName = MoveVariable.VariableName;
            }
            else if (MoveVariable.IsConstant)
            {
                _moveVariableName = MoveVariable.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
            }
        }
    }

    public override ActionResult Execute(AI ai)
    {
        // Fail if our team or variable are missing
        if (string.IsNullOrEmpty(_enemyVariableName))
            return ActionResult.FAILURE;
        if (string.IsNullOrEmpty(_moveVariableName))
            return ActionResult.FAILURE;

        //Vector3 attackpos = harness.GetAttackPosition(slot);

        float attackDistance = AttackDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
        Vector3 _myEnemy = ai.WorkingMemory.GetItem<Vector3>("enemyPosition");// EnemyVariable.Evaluate<GameObject>(ai.DeltaTime, ai.WorkingMemory);
        //Vector3 attackpos = _myEnemy.transform.position + _myEnemy.transform.forward * attackDistance;
        Vector3 tDirection = (ai.Body.transform.position - _myEnemy).normalized;
        Vector3 attackpos = _myEnemy + tDirection * attackDistance;

        Debug.Log(attackpos);

        ai.WorkingMemory.SetItem(_moveVariableName, attackpos);

        if (NavigationManager.Instance.GraphsForPoints(ai.Kinematic.Position, attackpos, ai.Motor.MaxHeightOffset, NavigationManager.GraphType.Navmesh, ((BasicNavigator)ai.Navigator).GraphTags).Count == 0)
            return ActionResult.FAILURE;

        return ActionResult.SUCCESS;
    }
}