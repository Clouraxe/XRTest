using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : Item
{
    

    private bool isSpinning = false;
    private bool IsSpinning
    {
        set
        {
            OnSpin(value);
            isSpinning = value;
        }
        get
        {
            return isSpinning;
        }
    } //After thrown needs a force that is perpandicular to the velocity and needs 

    private float ogMass;
    private Animator _animator;

    protected override void OnStart()
    {
        
        ogMass = _rb.mass;
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsSpinning)
        {
            _rb.AddForce(0.75f * _rb.mass * -Physics.gravity);
            Vector3 forceDirection = Vector3.Cross(_rb.linearVelocity.normalized, transform.forward);
            _rb.AddForce(forceDirection * 2, ForceMode.Impulse);
            Debug.Log("force is " + forceDirection + " and pos is " + transform.position);
            Debug.DrawLine(transform.position, transform.position + (forceDirection * 10f), Color.green, 1f);
        }
    }


    // spin logic for the boomerangs
    public void OnBoomerangUnselected()
    {
        Debug.Log("boomarang is speed of " + _rb.linearVelocity + " with mag of " + _rb.linearVelocity.magnitude);

        if (_rb.linearVelocity.magnitude < 3) return;

        Debug.Log("thrown the rang");
        _rb.AddRelativeForce(Vector3.one * 10f, ForceMode.Impulse); //Make the force stronger
        IsSpinning = true;

        // CreateCircularPath(rb);



    }

    public void OnSpin(bool on)
    {
        if (on) {
            _rb.mass = 0.5f;
            _animator.SetBool("isSpinning", true);

        } else {
            _rb.mass = ogMass;
            _animator.SetBool("isSpinning", false);
        }
        
    }


    public void OnCollisionEnter(Collision cols)
    {
        IsSpinning = false;

    }


    public static void CreateCircularPath(Rigidbody body)
    {
        // Vector3 startDirection = body.linearVelocity;

        // int nodeCount = (int) startDirection.magnitude / 2;
        // Debug.Log("we have " + nodeCount +  " nodes to make");

        // //now I need to get the Z axis plane
        // Vector3 projStart = Vector3.ProjectOnPlane(startDirection, body.transform.right);
        // Vector3 baseproj = Vector3.ProjectOnPlane(Vector3.zero, body.transform.right);

        // // Get the next point that is 20 degrees away (let's just say)
        // float angle = Mathf.Deg2Rad * 20f;
        // Vector3 dir = new(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        // Vector3 newPoint = body.transform.position + dir;

        // testCreateSphere(newPoint);


    }


    // public static void testCreateSphere(Vector3 pos)
    // {
    //     GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //     sphere.transform.position = pos;
    // }
}
