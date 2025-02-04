using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bow : Item
{

    [SerializeField] private LineRenderer _bowLine;
    [SerializeField] private XRGrabInteractable _grabInteract;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private AudioResource _releaseSound;
    [SerializeField] private bool _instantBowPull = true;
    
    private Quiver quiver; 

    private bool isBowScndGrab;
    private bool isArrowAttached = false;
    private Vector3 grab2StartPointLocal;
    private Rigidbody arrow;

    private readonly float ARROW_REST_Z = 0.15f;

    // Update is called once per frame
    void Update()
    {
        if (isArrowAttached && isBowScndGrab) {
            var grabPoint = _grabInteract.interactorsSelecting[1].transform.position;
            Vector3 grabLocal = transform.InverseTransformPoint(grabPoint);
            Vector3 grabLocalDiff = grabLocal - grab2StartPointLocal;
            Vector3 linePointPos = _bowLine.GetPosition(1);
            float newZ = Mathf.Clamp(grabLocalDiff.z, -0.25f, 0);
            _bowLine.SetPosition(1, new(linePointPos.x, linePointPos.y, newZ));   
            var arrowPos = arrow.transform.localPosition;
            arrow.transform.localPosition = new(arrowPos.x, arrowPos.y, newZ + ARROW_REST_Z);  
        }

    }

    protected override void OnStart()
    {
        //Makes an arrow pool instance 
        Pooler<Arrow>.Instance.Initiate(_arrowPrefab, 3, 6);

        //Get quiver reference
        quiver = Camera.main.transform.GetComponentInChildren<Quiver>(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<XRDirectInteractor>(out var interactor)) {
            if (arrow == null && (interactor.firstInteractableSelected?.transform.TryGetComponent<Arrow>(out var arr) ?? false))

                AttachArrow(arr);
        }
    }


    public void OnBowFirstSelect()
    {
        quiver.gameObject.SetActive(true);
    }

    public void OnBowSelect() 
    {
        if (_grabInteract.interactorsSelecting.Count < 2) return;

        isBowScndGrab = true;
        grab2StartPointLocal = transform.InverseTransformPoint(_grabInteract.interactorsSelecting[1].transform.position);
    } 
    public void OnBowDeselect() {
        if (_grabInteract.interactorsSelecting.Count != 1) return;
        if (arrow == null) return;
        
        // Released one, the other is still grabbing
        isBowScndGrab = false;
        isArrowAttached = false;
        Vector3 linePointPos = _bowLine.GetPosition(1);
        _bowLine.SetPosition(1, new(linePointPos.x, linePointPos.y, 0));

        //now to calculate the speed
        arrow.transform.SetParent(null);
        arrow.linearVelocity = 100f * -linePointPos.z * arrow.transform.forward;
        arrow.GetComponent<Rigidbody>().useGravity = true;
        arrow.GetComponent<Arrow>().SetFlying(true);
        arrow = null;

        //Play the sound
        var audios = GetComponent<AudioSource>();
        audios.resource = _releaseSound;
        audios.Play();
        
    }

    public void OnBowLastDeselect() 
    {
        quiver.gameObject.SetActive(false);
    }

    private void AttachArrow(Arrow arr)
    {
        Debug.Log("Arrow attached");
        arrow = arr.GetComponent<Rigidbody>();
        var arrInteract = arr.GetComponent<XRGrabInteractable>();
        var selector = arrInteract.firstInteractorSelecting;
        arrInteract.interactionManager.CancelInteractableSelection((IXRSelectInteractable)arrInteract);

        arr.transform.SetParent(transform);
        arr.transform.SetLocalPositionAndRotation(new Vector3(0,0, ARROW_REST_Z), Quaternion.identity);
        arrow.linearVelocity = Vector3.zero;
        arrow.angularVelocity = Vector3.zero;
        arrow.useGravity = false;

        if (_instantBowPull) {
            _grabInteract.interactionManager.SelectEnter(selector, _grabInteract);
        }

        arrInteract.enabled = false;
        isArrowAttached = true;
    }


    
}
