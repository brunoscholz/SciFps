using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    GameOptions options;

    public List<Transform> spawnPoints = new List<Transform>();
    public List<Transform> teamRed = new List<Transform>();
    public List<Transform> teamBlue = new List<Transform>();
    public Transform playerPrefab;
    public Transform zombiePrefab;
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
