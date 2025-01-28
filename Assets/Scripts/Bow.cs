using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Bow : Item
{

    [SerializeField] private LineRenderer _bowLine;
    [SerializeField] private XRGrabInteractable _grabInteract;
    [SerializeField] private GameObject _arrowPrefab;

    private bool isGrabbing;
    private Rigidbody arrow;

    private readonly float ARROW_REST_Z = 0.25f;

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing) {
            //Find the point where the grab is happening
            var grabPoint = _grabInteract.interactorsSelecting[1].transform.position;
            Vector3 grabLocal = transform.InverseTransformPoint(grabPoint);
            Vector3 linePointPos = _bowLine.GetPosition(1);
            float newZ = Mathf.Clamp(grabLocal.z, -0.25f, 0);
            _bowLine.SetPosition(1, new(linePointPos.x, linePointPos.y, newZ));   
            var arrowPos = arrow.transform.localPosition;
            arrow.transform.localPosition = new(arrowPos.x, arrowPos.y, newZ + ARROW_REST_Z);     
        } 
    }



    public void OnBowSelect() {
        if (_grabInteract.interactorsSelecting.Count < 2) return;

        if (arrow == null) {
            arrow = Instantiate(_arrowPrefab, transform).GetComponent<Rigidbody>();
            arrow.transform.localPosition = new(0, 0, ARROW_REST_Z);
            arrow.useGravity = false;
        }
        isGrabbing = true;
    } 
    public void OnBowDeselect() {
        if (_grabInteract.interactorsSelecting.Count != 1) return;

        isGrabbing = false;
        Vector3 linePointPos = _bowLine.GetPosition(1);
        _bowLine.SetPosition(1, new(linePointPos.x, linePointPos.y, 0));

        if (arrow == null) return;
        
        if (Mathf.Abs(linePointPos.z) <= 0.01f) {
            Destroy(arrow.gameObject);
            arrow = null;
            return;
        }

        //now to calculate the speed
        arrow.transform.SetParent(null);
        arrow.linearVelocity = 100f * -linePointPos.z * arrow.transform.forward;
        arrow.GetComponent<Rigidbody>().useGravity = true;
        arrow.GetComponent<Arrow>().SetFlying(true);
        arrow = null;


        
    } 


    
}
