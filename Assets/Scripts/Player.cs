using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Game mode
    [SerializeField] int playersTeamID;
    public int teamID { get { return playersTeamID; } }

    Rigidbody playerRigidbody;
    #endregion

    #region weapons
    public List<Weapon> weapons;
    int currentWeapon = 0;
    int lastWeapon = 0;
    public Vector2 dropOffset;
    #endregion

    private void Start()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        SwitchWeapon(currentWeapon);

        playerRigidbody = GetComponent<Rigidbody>();
        if(playerRigidbody == null)
        {
            Debug.LogError("Player Rigidbody not found");
        }

    }

    public void SwitchWeapon(int weaponID, bool overrideLock = false)
    {
        if(!overrideLock && weapons[currentWeapon].isWeaponLocked == true)
        {
            return;
        }

        lastWeapon = currentWeapon;
        currentWeapon = weaponID;

        weapons[lastWeapon].gameObject.SetActive(false);
        weapons[currentWeapon].gameObject.SetActive(true);
    }

    public void PickUpWeapon(GameObject weaponObject, Vector3 originalLocation, int teamID, int weaponID, bool overrideLock = false)
    {
        SwitchWeapon(weaponID, overrideLock);

        weapons[weaponID].SetUp(teamID, weaponObject, originalLocation);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            DropWeapon(currentWeapon);
        }
    }

    public void DropWeapon(int weaponID)
    {
        if(weapons[weaponID].isWeaponDropable)
        {
            Vector3 forward = transform.forward;
            forward *= dropOffset.x;
            forward.y = dropOffset.y;

            //Vector3 dropLocation =  transform.position + dropOffset;
            Vector3 dropLocation = transform.position + forward;

            weapons[weaponID].DropWeapon(playerRigidbody, dropLocation);
            weapons[weaponID].worldWeaponGameObject.SetActive(true);

            SwitchWeapon(lastWeapon, true);
        }
    }

    //mostly for when we capture the flag
    public void ReturnWeapon(int weaponID)
    {
        if(weapons[weaponID].isWeaponDropable)
        {
            Vector3 returnLocation = weapons[weaponID].originalLocation;

            //return the flag
            weapons[weaponID].worldWeaponGameObject.transform.position = returnLocation;
            weapons[weaponID].worldWeaponGameObject.SetActive(true);

            //switch back to normal weapons
            SwitchWeapon(lastWeapon, true);
        }
    }

    public bool IsHolding(int weaponID)
    {
        if(currentWeapon == weaponID)
        {
            return true;
        }

        return false;
    }

    public int GetWeaponTeamID()
    {
        return weapons[currentWeapon].teamID;
    }
}
