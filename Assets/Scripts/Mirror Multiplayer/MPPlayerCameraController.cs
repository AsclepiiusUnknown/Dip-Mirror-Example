using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorMPlayer
{
    public class MPPlayerCameraController : NetworkBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Camera playerCamera;
        private Transform cameraTransform;
       // private CharacterController controller;

        float xRotation = 0f;

        private Controls controls;
        private Controls Controls
        {
            get
            {
                if(controls != null) { return controls; }
                return controls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            //playerCamera = GetComponentInChildren<Camera>();
            cameraTransform = playerCamera.transform;
            //controller = GetComponent<CharacterController>();


            cameraTransform.gameObject.SetActive(true);

            enabled = true;

            Controls.Player.Look.performed += ctx => MouseLook(ctx.ReadValue<Vector2>());

            Controls.Player.Shoot.performed += ctx => Shoot();
        }

        [ClientCallback]
        private void OnEnable() => Controls.Enable();
        [ClientCallback]
        private void OnDisable() => Controls.Disable();

        [Client]
        void MouseLook(Vector2 lookAxis)
        {
            //camera rotation
            xRotation -= lookAxis.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            //character rotation
            transform.Rotate(Vector3.up * lookAxis.x);
        }

        void Shoot() => CmdShoot(cameraTransform.position, cameraTransform.forward);

        [Command]
        void CmdShoot(Vector3 cameraPosition, Vector3 cameraForward)
        {
            Ray ray = new Ray(cameraPosition, cameraForward * 500);
            Debug.DrawRay(cameraPosition, cameraForward * 500, Color.red, 2f);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log("SERVER: player shot:" + hit.collider.name);
                if(hit.collider.CompareTag("Player"))
                {
                    Debug.Log( "player hit: " + hit.collider.GetComponent<NetworkIdentity>().netId);
                    RpcPlayerFiredEntity(GetComponent<NetworkIdentity>().netId, 
                                         hit.collider.GetComponent<NetworkIdentity>().netId,
                        hit.point, hit.normal);

                    //hit.collider.GetComponent<PlayerController>().Damage(weaponDamage, GetComponent<NetworkIdentity>().netId);
                }
            }
        }

        //things that would only run on the client, like visuals
        [ClientRpc]
        void RpcPlayerFiredEntity(uint shootID, uint targetID,Vector3 impactPos, Vector3 impactRot)
        {
            //Instantiate(bulletHolePrefab, impactPos + impactRot * 0.1f, Quaternion.LookRotation(impactRot), 
            //NetworkIdentity.spawned[targetID].transform);

            //Instantiate(bulletBloodFXPrefab, impactPos, Quaternion.LookRotation(impactRot));

            //NetworkIdentity.spawned[shooterID].GetComponent<PlayerControllerFPS>().MuzzleFlash();
        }

    }
}