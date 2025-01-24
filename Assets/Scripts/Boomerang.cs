using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

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

    private Vector3 throwTargetPos;
    private Vector3 throwSourcePos;

    private Animator _animator;
    private bool useCircleForce = false;

    protected override void OnStart()
    {
        
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        if (IsSpinning) {
            Debug.DrawLine(transform.position, transform.position + _rb.linearVelocity * 0.4f, Color.cyan, 15f);
            Debug.DrawLine(transform.position, throwTargetPos, Color.green, 15f);

            var mag = _rb.linearVelocity.magnitude;
            _rb.linearVelocity += 0.35f * mag * (throwTargetPos - transform.position).normalized;
            _rb.linearVelocity = _rb.linearVelocity.normalized * mag;

            Debug.Log("the previous velo is " + mag);
            Debug.Log("the unchanged velo is " + _rb.linearVelocity.magnitude);

        }
    }


    // spin logic for the boomerangs
    public void OnBoomerangUnselected()
    {
        Debug.Log("boomarang is speed of " + _rb.linearVelocity);

        if (_rb.linearVelocity.magnitude < 3) return;

        Debug.Log("thrown the rang");
           

        throwSourcePos = Camera.main.transform.position;
        throwTargetPos = throwSourcePos + _rb.linearVelocity.magnitude * 2.5f * Camera.main.transform.forward;
        Debug.DrawLine(Camera.main.transform.position, throwTargetPos, Color.yellow, 10f);
        
        _rb.angularVelocity = Vector3.zero;
        _rb.linearVelocity.Scale(new(1,0,1));
        _rb.linearVelocity *= 0.15f;
        IsSpinning = true;
    }

    public void OnSpin(bool on)
    {
        if (on) {

            _rb.useGravity = false;
            _animator.SetBool("isSpinning", true);

        } else {
            _animator.SetBool("isSpinning", false);
            _rb.useGravity = true;
            useCircleForce = false;
        }
        
    }





    public void OnCollisionEnter(Collision cols)
    {
        IsSpinning = false;

    }


    private float RoundOffToNearest15(float y)
    {
        return MathF.Round(y / 15) * 15;
    }

    #region legacy

    //void FixedUpdate()
    // {
    //     if (IsSpinning) {
    //         Debug.DrawLine(transform.position, transform.position + _rb.linearVelocity * 0.4f, Color.cyan, 15f);
    //     }
    //     if (useCircleForce)
    //     {
    //         Vector3 forceDirection = Vector3.Cross(_rb.linearVelocity.normalized, transform.forward);
    //         _rb.AddForce(forceDirection * 0.4f, ForceMode.Impulse);
    //         Debug.DrawLine(transform.position, transform.position + forceDirection, Color.green, 15f);
    //         Debug.DrawRay(transform.position, (transform.position.normalized + forceDirection.normalized), Color.red, 15f);
    //     }
    // }
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
    #endregion
   
}
