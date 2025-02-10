using UnityEngine;

public class Boomerang : Item
{
    [SerializeField] private float turnMultiplier = 1f;

    private Vector3 throwTargetPos;
    private Vector3 throwSourcePos;

    private BoomerangState _state = BoomerangState.Idle;

    public void OnBoomerangUnselected()
    {
        if (_rb.linearVelocity.magnitude < 3) return;

        throwSourcePos = Camera.main.transform.position;
        throwTargetPos = throwSourcePos + _rb.linearVelocity.magnitude * Camera.main.transform.forward;
        ChangeState(BoomerangState.FlyingTowardsTarget);
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case BoomerangState.FlyingTowardsTarget:
                GravitateTowards(throwTargetPos);

                if((throwTargetPos- transform.position).magnitude <= 1f)
                    ChangeState(BoomerangState.ReturningToSender);
                break;
            case BoomerangState.ReturningToSender:
                GravitateTowards(throwSourcePos);

                if((throwSourcePos - transform.position).magnitude <= 1f)
                    ChangeState(BoomerangState.Idle);
                break;
        }
    }

    private void GravitateTowards(Vector3 targetPosition)
    {
        Vector3 linearVelocity = _rb.linearVelocity;
        float linearVelocityMagnitude = linearVelocity.magnitude;

        Vector3 transformToTarget = targetPosition - transform.position;
        linearVelocity += transformToTarget.normalized * turnMultiplier;

        _rb.linearVelocity = linearVelocity.normalized * linearVelocityMagnitude;
    }



    private void ChangeState(BoomerangState newState)
    {
        _state = newState;
        bool isIdle = newState == BoomerangState.Idle;
        _rb.useGravity = isIdle;
        GetComponent<Animator>().SetBool("isSpinning", !isIdle);
        GetComponent<TrailRenderer>().enabled = !isIdle;
    }

    

    private enum BoomerangState
    {
        Idle,
        FlyingTowardsTarget,
        ReturningToSender
    }
}
