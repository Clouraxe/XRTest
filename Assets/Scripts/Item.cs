using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    protected Rigidbody _rb;
    [SerializeField] protected Collider col;

    protected virtual void OnStart(){}
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        OnStart();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision cols)
    {
        if (cols.gameObject.layer == LayerMask.NameToLayer("Stage")) Invoke(nameof(DestroyObject), 3f);
    }


    private void DestroyObject() => Destroy(gameObject);
}
