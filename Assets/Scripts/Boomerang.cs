using System;
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

    private Vector3 throwTargetPos;
    private Vector3 throwSourcePos;

    private Animator _animator;

    protected override void OnStart()
    {
        
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        if (IsSpinning) {
            Debug.DrawLine(transform.position, transform.position + _rb.linearVelocity.normalized, Color.cyan, 15f);
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
        
        _rb.linearVelocity.Scale(new(0,1,1));
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
        }
        
    }





    public void OnCollisionEnter(Collision cols)
    {
        IsSpinning = false;

    }


   
}
