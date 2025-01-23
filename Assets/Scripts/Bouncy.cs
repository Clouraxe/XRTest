using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bouncy : MonoBehaviour
{
    [SerializeField] private float bounciness = 250f;

    private Collider _col;
    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<Collider>();


    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision collision)
    {
        Bounce(collision);
    }
    
    
    
    public void Bounce(Collision cols)
    {
        var rb = cols.rigidbody;
        Vector3 cntNorm = cols.contacts[0].normal;
        Vector3 bounceForce = -cntNorm * bounciness;
        

        rb.AddForce(bounceForce, ForceMode.Impulse);

        Vector3.Reflect(rb.linearVelocity, cntNorm);
        
        
    }
}
