using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    [SerializeField] int teamID;

    GameModeCTF gameModeCTF;

    private void Start()
    {
        gameModeCTF = FindObjectOfType<GameModeCTF>();

        if(gameModeCTF == null)
        {
            Debug.LogError("Could not find GameModeCTF script");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if(player != null && gameModeCTF != null)
        {
            if(player.GetWeaponTeamID() != teamID)
            {
                return;
            }

            //slot 1 in player weapons is always flag
            //we should find a different way to find the flag
            //but for now this is fine.
            if(player.IsHolding(1))
            {
                gameModeCTF.AddScore(player.teamID, 1);
                player.ReturnWeapon(1);
            }
        }
    }
}
