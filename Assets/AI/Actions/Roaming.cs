using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class Roaming : RAINAction
{
    private int _previousPoint = 0;
    private RAIN.Navigation.RAINNavigator _path;
    private bool getNext = true;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        _path = ai.Navigator;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if(getNext)
        {
            if (_previousPoint > 4)
                _previousPoint = 1;

            if(_path != null)
            {
                //
            }
        }

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}