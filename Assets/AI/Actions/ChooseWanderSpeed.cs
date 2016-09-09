using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;
using RAIN.Navigation;
using RAIN.Motion;

[RAINAction("Choose Wander Speed")]
public class ChooseWanderSpeed : RAINAction
{
    public Expression CloseEnoughDistance = new Expression();
    public Expression MaxDistance = new Expression();
    public Expression WalkSpeed = new Expression();
    public Expression RunSpeed = new Expression();
    public Expression MoveTargetVariable = new Expression();
    public Expression MoveSpeedVariable = new Expression();

    public MoveLookTarget _target = new MoveLookTarget();


    private string _moveTargetVariableName = null;
    private string _moveSpeedVariableName = null;

    private bool _atTarget = false;

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

        if (tMaxDistance <= 0f)
            tMaxDistance = _defaultWanderDistance;

        Vector3 tDistanceVector = ai.Kinematic.Position - _target.Position;
        tDistanceVector.y = 0f;
        float tDistance = tDistanceVector.magnitude;

        if (tDistance > tMaxDistance)
        {
            _atTarget = false;
            ai.WorkingMemory.SetItem(_moveSpeedVariableName, tRunSpeed);
        }
        else if(!_atTarget && (tDistance > tCloseEnoughDistance))
        {
            ai.WorkingMemory.SetItem(_moveSpeedVariableName, tRunSpeed);
        }
        else
        {
            _atTarget = true;
            ai.WorkingMemory.SetItem(_moveSpeedVariableName, tWalkSpeed);
        }

        return ActionResult.SUCCESS;
    }
}