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
    public bool onlyAI = false;
}

public class TeamSets
{
    public static Dictionary<string, Color> Colors = new Dictionary<string, Color> { { "red", new Color(1f, 0, 0) }, { "blue", new Color(0, 0.877f, 1) } };
    public static Dictionary<string, Vector3> Spots = new Dictionary<string, Vector3> { { "red", new Vector3(-9, 0, 43f) }, { "blue", new Vector3(9, 0, -43f) } };

    public static Vector3 LIMBO_POSITION = new Vector3(-200, 0, -200);
}

public enum TeamName { blue, red }
public enum GameMode { TimedDM, CountDM, CaptureFlag }
public enum Dificulty { Easy, Medium, Hard }