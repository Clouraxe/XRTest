using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Bow : Item
{

    [SerializeField] private LineRenderer _bowLine;
    [SerializeField] private XRGrabInteractable _grabInteract;
    [SerializeField] private GameObject _arrowPrefab;

    private bool isGrabbing;
    private Rigidbody arrow;

    private readonly float ARROW_REST_X = -0.26f;

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing) {
            //Find the point where the grab is happening
            var grabPoint = _grabInteract.interactorsSelecting[1].transform.position;
            Vector3 grabLocal = transform.InverseTransformPoint(grabPoint);
            Vector3 linePointPos = _bowLine.GetPosition(1);
            float newX = Mathf.Clamp(grabLocal.x, 0, 0.25f);
            _bowLine.SetPosition(1, new(newX, linePointPos.y, linePointPos.z));   
            var arrowPos = arrow.transform.localPosition;
            arrow.transform.localPosition = new(ARROW_REST_X + newX, arrowPos.y, arrowPos.z);     
        } 
    }



    public void OnBowSelect() {
        if (_grabInteract.interactorsSelecting.Count < 2) return;

        if (arrow == null) {
            arrow = Instantiate(_arrowPrefab, transform).GetComponent<Rigidbody>();
            arrow.transform.localPosition = new(ARROW_REST_X, 0, 0);
            arrow.useGravity = false;
        }
        isGrabbing = true;
    } 
    public void OnBowDeselect() {
        if (_grabInteract.interactorsSelecting.Count != 1) return;

        isGrabbing = false;
        Vector3 linePointPos = _bowLine.GetPosition(1);
        _bowLine.SetPosition(1, new(0, linePointPos.y, linePointPos.z));

        if (arrow == null) return;
        
        if (linePointPos.x == 0) {
            Destroy(arrow);
            arrow = null;
            return;
        }

        //now to calculate the speed
        arrow.transform.SetParent(null);
        arrow.linearVelocity = 100f * linePointPos.x * arrow.transform.forward;
        arrow.transform.GetChild(0).eulerAngles = arrow.linearVelocity;
        arrow.GetComponent<Rigidbody>().useGravity = true;
        arrow.GetComponent<Arrow>().SetFlying(true);
        arrow = null;


        
    } 


    
}
