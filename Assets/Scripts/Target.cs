using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 5f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isGoingBack = false;

    public event Action OnPop;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        if (endPoint != null) {
            endPos = endPoint.transform.position;
            Destroy(endPoint.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (endPos == null) return;
        
        var dest = isGoingBack ? startPos : endPos;

        if (!Vector3.Equals(transform.position, dest))
        {
            // Calculate the distance to the destination
            float t = Mathf.Clamp01(speed * Time.deltaTime / Vector3.Distance(transform.position, dest)); 
            transform.position = Vector3.Lerp(transform.position, dest, t);
        }
        else isGoingBack = !isGoingBack; 
    }

    public void OnTriggerEnter(Collider other)
    {        
        OnPop?.Invoke();
        Destroy(gameObject);
    }
}
