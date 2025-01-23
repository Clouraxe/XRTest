using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    protected Rigidbody rb;

    protected virtual void OnStart(){}
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        OnStart();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
