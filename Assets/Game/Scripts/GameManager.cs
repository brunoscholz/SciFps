using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    GameOptions options;
    Hud hud;
    Pause pauseCtrl;

    public List<Transform> spawnPoints = new List<Transform>();
    public Dictionary<string, List<IPlayer>> Teams = new Dictionary<string, List<IPlayer>> { { "blue", new List<IPlayer>() }, { "red", new List<IPlayer>() } };
    public Transform playerPrefab;
    public Transform robotPrefab;
    public Transform chickenPrefab;
    public Transform kamikazePrefab;

    private List<Transform> models = new List<Transform>();

    public bool isPaused
    {
        get { return pauseCtrl.IsPaused; }
    }

    /// <summary>
    /// "blue" counts the dead red
    /// "red" counts the dead blue
    /// </summary>
    public Dictionary<string, int> Score = new Dictionary<string, int> { { "blue", 0 }, { "red", 0 } };
    public Dictionary<string, int> ScoreFlag = new Dictionary<string, int> { { "blue", 0 }, { "red", 0 } };
    public Dictionary<string, Vector3> TeamBase = new Dictionary<string, Vector3> { { "blue", new Vector3(9, 0, -32f) }, { "red", new Vector3(-9, 0, 32f) } };

    public TimeSpan timer;
    public int counter = 0;
    public int flags = 3;

    public bool gameRunning = false;

    void Awake()
    {
        // do the setup
        options = GetComponent<GameOptions>();
        hud = GetComponent<Hud>();
        models.Add(kamikazePrefab);
        models.Add(chickenPrefab);

        pauseCtrl = GameObject.FindObjectOfType<Pause>();

        Setup();
        CreateTeams();
        StartCoroutine(StartGame());
    }

    void Setup()
    {
        Score["blue"] = 0;
        Score["red"] = 0;

        if (options.GameMode == GameMode.TimedDM)
        {
            timer = TimeSpan.FromMinutes(options.maxTime);
            counter = 0;
            flags = 0;
            hud.blueFlags.transform.parent.gameObject.SetActive(false);
            hud.redFlags.transform.parent.gameObject.SetActive(false);
        }
        else if (options.GameMode == GameMode.CountDM)
        {
            timer = TimeSpan.Zero;
            counter = options.maxKills;
            flags = 0;
            hud.timer.text = "--:--";
            hud.blueFlags.transform.parent.gameObject.SetActive(false);
            hud.redFlags.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            timer = TimeSpan.Zero;
            counter = 0;
            flags = options.maxFlags;
            hud.timer.text = "--:--";
            hud.blueFlags.transform.parent.gameObject.SetActive(true);
            hud.redFlags.transform.parent.gameObject.SetActive(true);
        }

        if(options.onlyAI)
        {
            hud.crosshair.enabled = false;
            hud.weapon.SetActive(false);
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
            int rand = UnityEngine.Random.Range(0, 100) % 2;
            TeamName pTeam = (TeamName)rand;

            playerPrefab.gameObject.SetActive(true);
            Vector3 pos = TeamSets.Spots[pTeam.ToString()];
            pos.y = 1;
            playerPrefab.position = TeamSets.Spots[pTeam.ToString()];
            playerPrefab.eulerAngles = playerPrefab.eulerAngles + 180f * rand * Vector3.up;
            playerPrefab.name = pTeam.ToString() + "Human_01";

            HumanPlayer human = playerPrefab.GetComponent<HumanPlayer>();
            human.SetUpPlayer(pTeam, 0);
            human.SetCommander(gameObject);
            human.SetColors(hud);
            Teams[pTeam.ToString()].Add(human);
        }

        // for red team
        int index = Teams["red"].Count;
        for(int i = index; i < options.maxTeamMembers; i++)
        {
            Vector3 pos = TeamSets.Spots["red"];
            pos.x += 3 * i;

            if(options.maxTeamMembers > 5 && i > 4)
                pos.z += 3;

            int randModel = UnityEngine.Random.Range(0, models.Count);

            Transform go = Instantiate(kamikazePrefab, pos, Quaternion.identity) as Transform; //models[randModel]
            go.eulerAngles = playerPrefab.eulerAngles + 180f * Vector3.up;
            go.name = "red" + "Bot_0" + (i + 1);
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
                pos.z -= 3;

            int randModel = UnityEngine.Random.Range(0, models.Count);
            Transform go = Instantiate(kamikazePrefab, pos, Quaternion.identity) as Transform;
            go.name = "blue" + "Bot_0" + (i + 1);
            ZombiePlayer zombie = go.GetComponent<ZombiePlayer>();
            zombie.SetUpPlayer(TeamName.blue, i);
            zombie.SetCommander(gameObject);
            Teams["blue"].Add(zombie);
        }
    }

    public Vector3 GetTeamBase(TeamName t)
    {
        return TeamBase[t.ToString()];
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
        Score[player.Team.ToString()]++;
    }

    public void Respawn(IPlayer player)
    {
        Vector3 pos = TeamSets.Spots[player.Team.ToString()];
        pos.x += 3 * player.teamID;

        if (options.maxTeamMembers > 5 && player.teamID > 4)
            pos.y += 3;


        int randModel = UnityEngine.Random.Range(0, models.Count);
        Transform go = Instantiate(kamikazePrefab, pos, Quaternion.identity) as Transform; //models[randModel]
        go.name = player.Team.ToString() + "Bot_0" + (player.teamID + 1);
        ZombiePlayer zombie = go.GetComponent<ZombiePlayer>();
        zombie.SetUpPlayer(player.Team, player.teamID);
        zombie.SetCommander(gameObject);

        Teams[player.Team.ToString()][player.teamID] = zombie;
        Teams[player.Team.ToString()][player.teamID].SetAction("start", true);
        player.Eliminate();
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

        hud.redScore.text = Score["blue"].ToString();
        hud.blueScore.text = Score["red"].ToString();

        UpdateTimer();

        // if > 25 kills, or time, or whatever
        // end game
    }

    void UpdateTimer()
    {
        if (options.GameMode == GameMode.TimedDM)
        {
            timer -= TimeSpan.FromSeconds(Time.deltaTime);

            hud.timer.text = string.Format("{0:D2}:{1:D2}", timer.Minutes, timer.Seconds);

            if (timer < TimeSpan.Zero)
            {
                EndGame();
                timer = TimeSpan.Zero;
            }
        }
    }

    void UpdateCounter()
    {
        if (options.GameMode == GameMode.CountDM)
        {
            if (Score["blue"] >= counter || Score["red"] >= counter)
            {
                EndGame();
            }
        }
    }

    void UpdateFlagger()
    {
        if (options.GameMode == GameMode.CaptureFlag)
        {
            if (ScoreFlag["blue"] >= flags || ScoreFlag["red"] >= flags)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        // deactivate all
    }
}
