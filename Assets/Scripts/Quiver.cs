using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Quiver : MonoBehaviour
{
    [SerializeField] private Collision _col;
    [SerializeField] private XRSimpleInteractable _interactable;

    private Arrow grabbedArrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnQuiverGrabbed()
    {
        var selector = _interactable.firstInteractorSelecting;
        _interactable.interactionManager.CancelInteractableSelection((IXRSelectInteractable)_interactable);

        grabbedArrow = Pooler<Arrow>.Instance.Get();
        grabbedArrow.transform.position = selector.transform.position;

        _interactable.interactionManager.SelectEnter(selector, grabbedArrow.GetComponent<XRGrabInteractable>());
    }

}
