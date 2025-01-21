using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

public class HandAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private InputActionProperty gripAction;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        anim.SetFloat("Trigger", triggerAction.action.ReadValue<float>());
        anim.SetFloat("Grip", gripAction.action.ReadValue<float>());
    }
}
