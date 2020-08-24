using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 20f;
    public float range = 100f;
    public float gunForce = 30f;


    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;
    public float fireRate = 15f;
    private void Start()
    {
        if (fpsCamera == null)
        {
            fpsCamera = GetComponent<Camera>();
        }

        if (fpsCamera == null)
        {
            fpsCamera = Camera.main;
        }
    }

    private void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1f / fireRate);
            Shoot();
        }
    }
    void Shoot()
    {
        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;
        if(Physics.Raycast(fpsCamera.transform.position,
            fpsCamera.transform.forward,
            out hit,
            range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(damage);
            }


            if(impactEffect != null)
            {

                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                impact.transform.SetParent(hit.transform);
                Destroy(impact, 10f);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * gunForce);
            }
        }
    }
}
