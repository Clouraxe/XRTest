using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : Item
{

    // Update is called once per frame
    void Update()
    {
        
    }


    // spin logic for the boomerangs
    public void OnBoomerangUnselected()
    {
        Debug.Log("boomarang is speed of " + rb.linearVelocity + " with mag of " + rb.linearVelocity.magnitude);

        if (rb.linearVelocity.magnitude > 15) Debug.Log("thrown the rang");

        rb.AddTorque(0, 0, 10, ForceMode.Force);

        CreateCircularPath(rb.linearVelocity);
    }






    public static void CreateCircularPath(Vector3 startDirection)
    {
        Vector3 dirNorm = startDirection.normalized;

        int nodeCount = (int) startDirection.sqrMagnitude;
        Debug.Log("we have " + nodeCount +  " nodes to make");

        
    }
}
