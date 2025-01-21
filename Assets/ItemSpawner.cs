using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SpawnItem()
    {
        var interact = GetComponent<XRSimpleInteractable>();
        var selector = interact.firstInteractorSelecting;
        interact.interactionManager.CancelInteractableSelection((IXRSelectInteractable)interact);
        
        var itm = Instantiate(spawnItem, this.transform.position + new Vector3(0, 0.1f , 0), spawnItem.transform.rotation, this.transform.parent);

        interact.interactionManager.SelectEnter(selector, itm.GetComponent<XRGrabInteractable>());
    }
}
