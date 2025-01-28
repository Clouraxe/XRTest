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
        
        var itm = Instantiate(spawnItem, selector.transform.position, spawnItem.transform.rotation, null);

        interact.interactionManager.SelectEnter(selector, itm.GetComponent<XRGrabInteractable>());
    }
}
