using System;
using UnityEngine;

public class ItemPedestalContainer : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float height = 0.05f;
    [SerializeField] private float restockTime = 3f;

    private GameObject item;
    private GameObject itemCopy;
    private float startY;

    public event Action OnItemRemoved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        item = transform.GetChild(0).gameObject;
        itemCopy = Instantiate(item, transform, false);
        itemCopy.SetActive(false);

        startY = transform.position.y;

        TogglePhysics(item, false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float newY = startY + Mathf.Sin(Time.time * speed) * height;
        transform.position = new(pos.x, newY, pos.z);
    }

    void OnTransformChildrenChanged()
    {
        if (transform.childCount == 1) {

            TogglePhysics(item, true);
            Invoke(nameof(RespawnItem), restockTime);

            //Starts timer
            OnItemRemoved?.Invoke();
        }
    }

    public void RespawnItem()
    {
        itemCopy.name = item.name ?? itemCopy.name;
        item = itemCopy;
        item.SetActive(true);

        itemCopy = Instantiate(item, transform, false);
        itemCopy.SetActive(false);

        TogglePhysics(item, false);
    }

    private void TogglePhysics(GameObject obj, bool on)
    {
        if (obj.TryGetComponent<Rigidbody>(out var rb)) {
            rb.useGravity = on;
            rb.isKinematic = !on;
        }
    }
}
