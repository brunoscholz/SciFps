using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Navigation;
using RAIN.Motion;

[RAINAction("Choose Wander Position")]
public class ChooseWanderPosition : RAINAction
{
    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// WanderDistance is the max range to use when picking a wander target
    /// </summary>

    public Expression CloseEnoughDistance = new Expression();
    public Expression MaxDistance = new Expression();
    public Expression WalkSpeed = new Expression();
    public Expression RunSpeed = new Expression();
    public Expression MoveTargetVariable = new Expression();
    public Expression MoveSpeedVariable = new Expression();

    public Vector3 _target = Vector3.zero;


    private string _moveTargetVariableName = null;
    private string _moveSpeedVariableName = null;

    private bool _atTarget = false;

    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// StayOnGraph is a boolean (true/false) that indicates whether the wander target must be on the nav graph
    /// </summary>
    public Expression StayOnGraph = new Expression();

    /// <summary>
    /// The default wander distance to use when the WanderDistance is invalid
    /// </summary>
    private float _defaultWanderDistance = 10f;

    public override void Start(AI ai)
    {
        base.Start(ai);

        _moveSpeedVariableName = null;
        if (MoveSpeedVariable.IsValid)
        {
            if (MoveSpeedVariable.IsVariable)
            {
                _moveSpeedVariableName = MoveSpeedVariable.VariableName;
            }
            else if (MoveSpeedVariable.IsConstant)
            {
                _moveSpeedVariableName = MoveSpeedVariable.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
            }
        }

        _moveTargetVariableName = null;
        if (MoveTargetVariable.IsValid)
        {
            if (MoveTargetVariable.IsVariable)
            {
                _moveTargetVariableName = MoveTargetVariable.VariableName;
            }
            else if (MoveTargetVariable.IsConstant)
            {
                _moveTargetVariableName = MoveTargetVariable.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
            }
        }
    }

    public override ActionResult Execute(AI ai)
    {
        if (string.IsNullOrEmpty(_moveTargetVariableName))
            return ActionResult.FAILURE;
        if (string.IsNullOrEmpty(_moveSpeedVariableName))
            return ActionResult.FAILURE;

        float tMaxDistance = Mathf.Max(ai.Motor.DefaultCloseEnoughDistance * 2f, MaxDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory));
        float tCloseEnoughDistance = Mathf.Max(ai.Motor.DefaultCloseEnoughDistance, CloseEnoughDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory));
        float tWalkSpeed = WalkSpeed.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
        float tRunSpeed = RunSpeed.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

        float tWanderDistance = 0f;
        tWanderDistance = tMaxDistance;

        if (tWanderDistance <= 0f)
            tWanderDistance = _defaultWanderDistance;

        if (_atTarget || _target == Vector3.zero)
        {
            Vector3 tDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
            tDirection *= tWanderDistance;

            Vector3 tDestination = ai.Kinematic.Position + tDirection;
            if (StayOnGraph.IsValid && (StayOnGraph.Evaluate<bool>(ai.DeltaTime, ai.WorkingMemory)))
            {
                if (NavigationManager.Instance.GraphForPoint(tDestination, ai.Motor.MaxHeightOffset, NavigationManager.GraphType.Navmesh, ((BasicNavigator)ai.Navigator).GraphTags).Count == 0)
                    return ActionResult.FAILURE;
            }

            _target = tDestination;
            ai.WorkingMemory.SetItem<Vector3>(_moveTargetVariableName, tDestination);
        }

        Vector3 tDistanceVector = ai.Kinematic.Position - _target;
        tDistanceVector.y = 0f;
        float tDistance = tDistanceVector.magnitude;

        if (tDistance > tMaxDistance)
        {
            _atTarget = false;
            ai.WorkingMemory.SetItem<float>(_moveSpeedVariableName, tRunSpeed);
        }
        else if (!_atTarget && (tDistance > tCloseEnoughDistance))
        {
            ai.WorkingMemory.SetItem<float>(_moveSpeedVariableName, tRunSpeed);
        }
        else
        {
            _atTarget = true;
            ai.WorkingMemory.SetItem<float>(_moveSpeedVariableName, tWalkSpeed);
        }

        return ActionResult.SUCCESS;
    }
}