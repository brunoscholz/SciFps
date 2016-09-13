using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

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

    public bool gameRunning = false;

    void Awake()
    {
        // do the setup
        options = GetComponent<GameOptions>();
        Setup();
        CreateTeams();
        StartCoroutine(StartGame());
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
    }

    void CreateTeams()
    {
        // Clear both teams
        Teams["blue"].Clear();
        Teams["red"].Clear();

        if (!options.onlyAI)
        {
            // Create player in a random Team
            TeamName pTeam = (TeamName)UnityEngine.Random.Range(0, 1);
            Transform go = Instantiate(playerPrefab, TeamSets.Spots[pTeam.ToString()], Quaternion.identity) as Transform;
            HumanPlayer human = go.GetComponent<HumanPlayer>();
            human.SetUpPlayer(pTeam, 0);
            human.SetColors(GetComponent<Hud>());
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
            zombie.SetUpPlayer(TeamName.red, i);
            zombie.SetCommander(gameObject);
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
            zombie.SetUpPlayer(TeamName.blue, i);
            zombie.SetCommander(gameObject);
            Teams["blue"].Add(zombie);
        }
    }

    IEnumerator StartGame ()
    {
        yield return null;

	    foreach(IPlayer p in Teams["red"].Concat(Teams["blue"]))
        {
            p.SetAction("start", true);
        }

        gameRunning = true;
	}
	
    void ResetGame()
    {
        gameRunning = false;
        CreateTeams();
        StartCoroutine(StartGame());
    }

    public void ReportDown(IPlayer player)
    {
        Vector3 pos = TeamSets.Spots[player.Team.ToString()];
        pos.x += 3 * player.teamID;

        if (options.maxTeamMembers > 5 && player.teamID > 4)
            pos.y += 3;

        player.Body.position = pos;
        player.Respawn();
    }

    public void AskForHelp(IPlayer needed, Vector3 target)
    {
        foreach (IPlayer p in Teams[needed.Team.ToString()])
        {
            p.SetAction("help", target);
        }
    }

	void Update ()
    {
        if (!gameRunning)
            return;

        if (options.GameMode == GameMode.TimedDM)
        {
            UpdateTimer();
        }
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
