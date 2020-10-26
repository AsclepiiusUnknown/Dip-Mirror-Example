using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    List<Rigidbody> bodies;

    public bool test = false;
    // Start is called before the first frame update
    void Start()
    {
        SetKinematic(true);

    }
    private void Update()
    {
        if(test)
        {
            Death();
        }
    }
    void SetKinematic(bool value)
    {
        bodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = value;
        }
    }

    void Death()
    {
        SetKinematic(false);
        Animator anim;
        if(TryGetComponent<Animator>(out anim))
        {
            anim.enabled = false;
        }
    }
}
