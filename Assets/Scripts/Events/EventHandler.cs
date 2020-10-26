using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    [SerializeField]
    private EventManager eventManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EventManager.onGameStart += ChangeColour;
        eventManager.onChangeName += DisplayName;
        EventManager.onGameStart += LogStuff;
    }

    private void OnDisable()
    {
        EventManager.onGameStart -= ChangeColour;
        eventManager.onChangeName -= DisplayName;
        EventManager.onGameStart -= LogStuff;
    }

    void LogStuff()
    {
        Debug.Log("SDJKHSDFJKHDFJKHDJKFHjk");
    }

    void DisplayName(string name)
    {
        Debug.Log(name);
    }

    void ChangeColour()
    {
        spriteRenderer.color = Color.red;
    }

}
