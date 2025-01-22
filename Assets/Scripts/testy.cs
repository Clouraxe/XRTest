using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testy : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(-200f, 0f, 0f), ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
