using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    CharacterJoint joint;

    Vector3 previousForce = Vector3.zero;

    [SerializeField]
    float Score;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 forceDifference = joint.currentForce - previousForce;

        float forceDifferenceMagnitude = forceDifference.magnitude;

        if(forceDifferenceMagnitude > 100f)
        {
            Score += forceDifferenceMagnitude;
        }

        previousForce = joint.currentForce;
    }
}
