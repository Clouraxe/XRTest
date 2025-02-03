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
    private Vector3 throwMidPoint;
    private float throwStartAngle;
    private float throwSpeed = 1f;
    private float throwTime = 0f;
    private float throwRadius;
    private bool isArcing = false;

    private Animator _animator;

    protected override void OnStart()
    {
        
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        if (isArcing) {
            throwTime += Time.deltaTime;
            // Debug.DrawLine(transform.position, transform.position + _rb.linearVelocity.normalized, Color.cyan, 15f);
            // Debug.DrawLine(transform.position, throwTargetPos, Color.green, 15f);

            // var mag = _rb.linearVelocity.magnitude;
            // _rb.linearVelocity += 0.35f * mag * (throwTargetPos - transform.position).normalized;
            // _rb.linearVelocity = _rb.linearVelocity.normalized * mag;

            // Debug.Log("the previous velo is " + mag);
            // Debug.Log("the unchanged velo is " + _rb.linearVelocity.magnitude);

            Debug.DrawLine(transform.position, throwMidPoint, Color.red, 5f);
            
            

            float z = throwMidPoint.z + Mathf.Cos((throwTime + throwStartAngle) * throwSpeed) * throwRadius;
            float x = throwMidPoint.x + Mathf.Sin((throwTime + throwStartAngle) * throwSpeed) * throwRadius;


            transform.localPosition = new(x, transform.localPosition.y, z);
            Debug.DebugBreak();
        }
    }


    // spin logic for the boomerangs
    public void OnBoomerangUnselected()
    {
        Debug.Log("boomarang is speed of " + _rb.linearVelocity);
        
        if (_rb.linearVelocity.magnitude < 3) return;

        Debug.Log("thrown the rang");
        IsSpinning = true;
           

        throwSourcePos = Camera.main.transform.position;
        throwTargetPos = throwSourcePos + _rb.linearVelocity.magnitude * 2.5f * Camera.main.transform.forward;
        throwMidPoint = throwSourcePos + (throwTargetPos - throwSourcePos) / 2f;
        throwRadius = Vector3.Distance(throwSourcePos, throwTargetPos) / 2.0f;
        throwTime = 0f;
        Debug.DrawLine(throwSourcePos, throwTargetPos, Color.yellow, 10f);
        Debug.DrawLine(throwSourcePos, throwMidPoint, Color.cyan, 5f);

        GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = throwMidPoint;

        transform.eulerAngles = new(transform.eulerAngles.x, transform.eulerAngles.y, (int)(transform.eulerAngles.z / 15) * 15);
        _rb.maxAngularVelocity = 0;
        // _rb.linearVelocity.Scale(new(0,1,1));
        // _rb.linearVelocity *= 0.15f;
        Invoke(nameof(StartArc), 1f);
    }

    private void StartArc()
    {
        isArcing = true;
        _rb.linearVelocity = Vector3.zero;
        throwStartAngle = Mathf.Asin((transform.localPosition.z - throwMidPoint.z) / throwRadius) / throwSpeed;
    }
    public void OnSpin(bool on)
    {
        if (on) {

            _rb.useGravity = false;
            _animator.SetBool("isSpinning", true);

        } else {
            _animator.SetBool("isSpinning", false);
            _rb.useGravity = true;
            isArcing = false;
        }
        
    }





    public void OnCollisionEnter(Collision cols)
    {
        IsSpinning = false;

    }


   
}
