using UnityEngine;
using System.Collections;
using RAIN.Entities.Aspects;
using System;
using RAIN.Serialization;
using RAIN.Core;

[RAINElement("Team Aspect")]
[RAINSerializableClass]
public class TeamAspect : VisualAspect
{
    [RAINSerializableField]
    public TeamName team = TeamName.blue;

    public TeamAspect()
    {
        AspectName = "team";
    }

    public TeamAspect(TeamName t)
    {
        AspectName = "team";
        team = t;
    }

    public override string AspectType
    {
        get
        {
            return "visual";
        }
    }
}
