using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastlePart : MonoBehaviour
{
    [Header("Inscribed")]
    public float breakForce = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > breakForce)
        {
            BreakApart();
        }
    }

    void BreakApart()
    {
        Destroy(gameObject);
    }
}
