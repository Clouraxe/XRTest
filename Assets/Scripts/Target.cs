using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Api;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class Target : MonoBehaviour
{
    public enum MoveType {
        Default,
        Circular
    }
    
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private MoveType moveType;

    [Header("Circle Movement")]
    [SerializeField] private Axis circleAround; //For circular motion
    [SerializeField] private float circleRadius;

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
        switch(moveType) {
            case MoveType.Default:
                MoveLinear();
                break;
            
            case MoveType.Circular:
                MoveCircle();
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>() == null) return;
        
        OnPop?.Invoke();
        Destroy(gameObject);
    }
    
    
    private void MoveLinear()
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
    
    private void MoveCircle()
    {
        Vector3 newPos = transform.position;
        float x, y, z;
        switch(circleAround) {
            case Axis.X:
                y = startPos.y + Mathf.Cos(Time.time * speed) * circleRadius;
                z = startPos.z + Mathf.Sin(Time.time * speed) * circleRadius;
                newPos = new(startPos.x, y, z);
                break;
            
            case Axis.Y:
                x = startPos.x + Mathf.Cos(Time.time * speed) * circleRadius;
                z = startPos.z + Mathf.Sin(Time.time * speed) * circleRadius;
                newPos = new(x, startPos.y, z);
                break;
                
            case Axis.Z:
                x =  startPos.x + Mathf.Cos(Time.time * speed) * circleRadius;
                y =  startPos.y + Mathf.Sin(Time.time * speed) * circleRadius;
                newPos = new(x, y, startPos.z);
                break;
                
            default:
                Debug.LogError("No axis specified!!");
                break;
        }

        transform.position = newPos; 
        
        
    }
}
