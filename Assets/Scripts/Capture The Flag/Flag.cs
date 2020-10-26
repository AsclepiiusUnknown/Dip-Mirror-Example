using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] int teamID;
    public Vector3 originalLocation;

    const int weaponID = 1;

    void Start()
    {
        originalLocation = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if(player != null)
        { 
            if(player.teamID == teamID)
            {//players cant pick up their own flag

                //return the flag
                return;
            }
            Debug.Log("Capture Flag");

            player.PickUpWeapon(gameObject, originalLocation, teamID, weaponID);

            gameObject.SetActive(false);
        }
    }
}
