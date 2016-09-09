using RAIN.Core;
using RAIN.Serialization;

[RAINSerializableClass]
public class TeamElement : CustomAIElement
{
    [RAINSerializableField]
    public TeamName Team = TeamName.blue;

    //[RAINSerializableField(Visibility = FieldVisibility.ShowAdvanced, ToolTip = "An advanced float")]
    //private float _advancedFloat = 4.5f;

    public TeamElement() { }
    public TeamElement(TeamName t)
    {
        Team = t;
    }

    public override void AIInit()
    {
        base.AIInit();

        // This is equivilent to an Awake call
    }
}