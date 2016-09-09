using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    GameOptions options;

    public List<Transform> spawnPoints = new List<Transform>();
    public Dictionary<string, List<IPlayer>> Teams = new Dictionary<string, List<IPlayer>> { { "blue", new List<IPlayer>() }, { "red", new List<IPlayer>() } };
    public Transform playerPrefab;
    public Transform robotPrefab;
    public Transform chickenPrefab;
    public Transform kamikazePrefab;

    public int redKills = 0;
    public int blueKills = 0;

    public TimeSpan timer;
    public int counter = 0;
    public int flags = 3;

    void Awake()
    {
        // do the setup
        options = GetComponent<GameOptions>();
        Setup();
        CreateTeams();
    }

    void Setup()
    {
        if (options.GameMode == GameMode.TimedDM)
        {
            timer = TimeSpan.FromMinutes(options.maxTime);
            counter = 0;
            flags = 0;
        }
        else if (options.GameMode == GameMode.CountDM)
        {
            timer = TimeSpan.Zero;
            counter = options.maxKills;
            flags = 0;
        }
        else
        {
            timer = TimeSpan.Zero;
            counter = 0;
            flags = options.maxFlags;
        }

        // Clear both teams
        Teams["blue"].Clear();
        Teams["red"].Clear();
    }

    void CreateTeams()
    {
        if(!options.onlyAI)
        {
            // Create player in a random Team
            TeamName pTeam = (TeamName)UnityEngine.Random.Range(0, 1);
            HumanPlayer human = (Instantiate(playerPrefab, TeamSets.Spots[pTeam.ToString()], Quaternion.identity) as GameObject).GetComponent<HumanPlayer>();
            human.SetUpPlayer(pTeam);
            Teams[pTeam.ToString()].Add(human);
        }

        // for red team
        int index = Teams["red"].Count;
        for(int i = index; i < options.maxTeamMembers; i++)
        {
            Vector3 pos = TeamSets.Spots["red"];
            pos.x += 3 * i;

            if(options.maxTeamMembers > 5 && i > 4)
                pos.y += 3;

            Transform go = Instantiate(kamikazePrefab, pos, Quaternion.identity) as Transform;
            ZombiePlayer zombie = go.GetComponent<ZombiePlayer>();
            zombie.SetUpPlayer(TeamName.red);
            Teams["red"].Add(zombie);
        }

        // for blue team
        index = Teams["blue"].Count;
        for (int i = index; i < options.maxTeamMembers; i++)
        {
            Vector3 pos = TeamSets.Spots["blue"];
            pos.x -= 3 * i;

            if (options.maxTeamMembers > 5 && i > 4)
                pos.y -= 3;

            Transform go = Instantiate(kamikazePrefab, pos, Quaternion.identity) as Transform;
            ZombiePlayer zombie = go.GetComponent<ZombiePlayer>();
            zombie.SetUpPlayer(TeamName.blue);
            Teams["blue"].Add(zombie);
        }
    }

    void StartGame ()
    {
	    // instantiate teams, start game!
        
	}
	
	void Update ()
    {
        // if > 25 kills, or time, or whatever
        // end game
    }

    void UpdateTimer()
    {
        timer -= TimeSpan.FromSeconds(Time.deltaTime);

        if (timer < TimeSpan.Zero)
        {
            EndGame();
            timer = TimeSpan.Zero;
        }

        //timeText.text = string.Format("{0:D2}:{1:D2}", timer.Minutes, timer.Seconds);
    }

    void EndGame()
    {

    }
}
