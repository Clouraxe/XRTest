using System;
using UnityEngine;
using UnityEngine.Animations;


public class Target : MonoBehaviour
{
    public enum MoveType {
        Default,
        Circular,
        Multi
    }

    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private MoveType moveType;

    [Header("Circle Movement")]
    [SerializeField] private Axis circleAround; //For circular motion

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isGoingBack = false;
    private int multiCurrentIndex = 0; //The current child the target is moving to (multi point)

    public event Action OnPop;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        if (endPoint != null) {
            endPos = endPoint.transform.position;
            endPoint.gameObject.SetActive(false);
        }
        
        if (moveType == MoveType.Multi) {
            foreach (Transform tra in transform.parent) {
                if (tra == transform) continue;
                tra.gameObject.SetActive(false);
            }
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
            
            case MoveType.Multi:
                MoveMulti();
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>() == null) return;
        GetComponent<AudioSource>().Play();
        
        OnPop?.Invoke();
        GetComponent<MeshRenderer>().enabled = false;
        Invoke(nameof(DestroyObj), 1f);
    }

    private void DestroyObj()
    {
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
        Vector3 newPos = endPos;
        var radius = Mathf.Abs((endPos - startPos).magnitude);
        float x, y, z;
        switch(circleAround) {
            case Axis.X:
                y = endPos.y + Mathf.Cos(Time.time * speed) * radius;
                z = endPos.z + Mathf.Sin(Time.time * speed) * radius;
                newPos = new(endPos.x, y, z);
                break;
            
            case Axis.Y:
                x = endPos.x + Mathf.Cos(Time.time * speed) * radius;
                z = endPos.z + Mathf.Sin(Time.time * speed) * radius;
                newPos = new(x, endPos.y, z);
                break;
                
            case Axis.Z:
                x =  endPos.x + Mathf.Cos(Time.time * speed) * radius;
                y =  endPos.y + Mathf.Sin(Time.time * speed) * radius;
                newPos = new(x, y, endPos.z);
                break;
                
            default:
                Debug.LogError("No axis specified!!");
                break;
        }

        transform.position = newPos; 
    }
    
    private void MoveMulti()
    {
        if (!Vector3.Equals(transform.position, endPos))
        {
            // Calculate the distance to the destination
            float t = Mathf.Clamp01(speed * Time.deltaTime / Vector3.Distance(transform.position, endPos)); 
            transform.position = Vector3.Lerp(transform.position, endPos, t);
        } else {
            multiCurrentIndex++;
            if (multiCurrentIndex == transform.GetSiblingIndex()) multiCurrentIndex++;
            if (multiCurrentIndex == transform.parent.childCount) multiCurrentIndex = -1;

            endPos = multiCurrentIndex == -1 ? startPos : transform.parent.GetChild(multiCurrentIndex).position;
        }
    }
}
