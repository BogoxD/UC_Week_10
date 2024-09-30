using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableObject : MonoBehaviour
{
    public float forceAmmount;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void HitObjectWithForce(Vector3 direciton, float forceAmmount, ForceMode forceMode)
    {
        rb.AddForce(direciton * forceAmmount, forceMode);
    }
    public void HitObjectWithForce(Vector3 direciton, ForceMode forceMode)
    {
        rb.AddForce(direciton * forceAmmount, forceMode);
    }
}
