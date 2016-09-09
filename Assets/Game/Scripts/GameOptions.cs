using UnityEngine;
using System.Collections.Generic;

public class GameOptions : MonoBehaviour
{
    public GameMode GameMode = GameMode.TimedDM;
    public int maxTime = 10;
    public int maxKills = 20;
    public int maxFlags = 3;

    public Dificulty Dificulty = Dificulty.Medium;

    public int maxTeamMembers = 5;
}

public class TeamColors
{
    public static Dictionary<string, Color> Colors = new Dictionary<string, Color> { { "red", Color.red }, { "blue", Color.blue } };
}

public enum TeamName { red, blue }
public enum GameMode { TimedDM, CountDM, CaptureFlag }
public enum Dificulty { Easy, Medium, Hard }