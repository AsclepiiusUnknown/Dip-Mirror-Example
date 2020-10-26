using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    
    public delegate void OnGameStart();
    public static event OnGameStart onGameStart;

    public delegate void OnChangeName(string name);
    public event OnChangeName onChangeName;

    float startTime = 3f;
    
    public UnityEvent testEvent;

    private void Start()
    {
        RegisteringToEvent();
        StartCoroutine(GameStarts());

        //triggering the event
        onChangeName("New Name");

        testEvent.AddListener(SpawnPlayers);
        testEvent.Invoke();

    }

    IEnumerator  GameStarts()
    {
        yield return new WaitForSeconds(startTime);
        onGameStart();
    }


    //somewhere else, but its here for an example
    void RegisteringToEvent()
    {
        onGameStart += SpawnPlayers;
        onGameStart += ResetScores;

        //onGameStart -= SpawnPlayers;
    }

    void SpawnPlayers()
    {
        Debug.Log("Spawning Players");
    }

    void ResetScores()
    {
        Debug.Log("Reseting Scores");
    }
    //--------------

}
