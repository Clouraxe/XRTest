using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] private Rigidbody _rb;

    private bool isFlying = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying) {
            // Debug.DrawLine(transform.position, transform.position + _rb.linearVelocity, Color.blue, 10f);
            // Debug.DrawLine(transform.position, transform.position + transform.GetChild(0).eulerAngles, Color.red, 10f);
            // Debug.Log(_rb.linearVelocity);
            transform.LookAt(transform.position +_rb.linearVelocity );
            // Debug.DebugBreak();
        }
    }



    public void SetFlying(bool val)
    {
        isFlying = val;
    }
}
