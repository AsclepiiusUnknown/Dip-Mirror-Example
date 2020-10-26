using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int teamID;
    public bool isWeaponLocked = false;
    public bool isWeaponDropable = false;

    public GameObject worldWeaponGameObject;
    public Vector3 originalLocation;

    public void SetUp(int teamID, GameObject worldGameObject, Vector3 originalLocation)
    {
        this.teamID = teamID;
        if(worldGameObject != null)
        {
            worldWeaponGameObject = worldGameObject;
        }

        this.originalLocation = originalLocation;
    }

    public void DropWeapon(Rigidbody player, Vector3 dropLocation)
    {
        Vector3 directionToDrop = dropLocation - Camera.main.transform.position;

        //ray to drop location
        Ray rayToDropLocation = new Ray(Camera.main.transform.position, directionToDrop);
        RaycastHit hit;

        if(Physics.Raycast(rayToDropLocation, out hit, directionToDrop.magnitude))
        {
            dropLocation = hit.point;
        }

        worldWeaponGameObject.transform.position = dropLocation;

        //raycast down to make sure the weapon doesn't fall through the floor
        Renderer rend = worldWeaponGameObject.GetComponent<Renderer>();
        if (rend != null)
        {
            Debug.Log("Dropping using render: " + rend.name);

            //set up our ray cast variables
            Vector3 topPoint = rend.bounds.center;
            topPoint.y += rend.bounds.extents.y;
            float height = rend.bounds.extents.y * 2;

            //raycast
            Ray rayDown = new Ray(topPoint, Vector3.down);
            RaycastHit downRayHit;
            if(Physics.Raycast(rayDown, out downRayHit, height * 1.1f))
            {
                dropLocation = downRayHit.point;
                dropLocation.y += rend.bounds.extents.y * 1.1f;
            }

            worldWeaponGameObject.transform.position = dropLocation;
        }
        else
        {
            Debug.LogError("Renderer not found for drop weapon");
        }

        Rigidbody weaponRigidBody = worldWeaponGameObject.GetComponent<Rigidbody>();
        if(weaponRigidBody != null && player != null)
        {
            weaponRigidBody.velocity = player.velocity;
            //if you are not using rigidbody, you would need the players movedirection *  speed
        }
    }
}
