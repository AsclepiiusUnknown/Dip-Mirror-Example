using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public int teamAmmount = 2;

    public List<Team> teams;
    //public List<Transform> spawnPoints;

    protected void Start()
    {
        Debug.Log("Setting up game");
        SetUpGame();
    }

    public void SetUpGame()
    {
        for(int teamID = 1; teamID <= teamAmmount ;teamID++)
        {
            teams.Add(new Team(teamID));
        }
    }

    public void AddScore(int teamID, int score)
    {
        foreach(Team team in teams)
        {
            if (team.teamID == teamID)
            {
                team.score += score;
                return;
            }
        }
    }
}

[System.Serializable]
public class Team
{
    public int score;
    public int teamID = 0;

    public Team(int teamID)
    {
        this.teamID = teamID;
    }
}