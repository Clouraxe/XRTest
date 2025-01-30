using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] private Rigidbody _rb;

    private bool isFlying = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying) {
            transform.LookAt(transform.position +_rb.linearVelocity );
        }
    }



    public void SetFlying(bool val)
    {
        isFlying = val;
    }

    void OnCollisionEnter(Collision cols)
    {
        
        if (cols.gameObject.GetComponent<Target>() == null)
        {
            isFlying = false;
            _rb.useGravity = false;
            _rb.maxAngularVelocity = 0;
            _rb.maxLinearVelocity = 0;
            Invoke(nameof(DestroyArrow), 3f);

            var audios = GetComponent<AudioSource>();
            audios.Play();
        }
    }


    private void DestroyArrow()
    {
        Destroy(this.gameObject);
    }
}
