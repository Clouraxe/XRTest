using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    //Components
    private AudioSource _audioSrc;
    private ParticleSystem _particleSys;
    private TrailRenderer _trailRender;

    private bool isFlying = false;

    void Start()
    {
        _trailRender.enabled = false;

        _audioSrc = GetComponent<AudioSource>();
        _particleSys = GetComponent<ParticleSystem>();
        _trailRender = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlying) {
            transform.LookAt(transform.position + _rb.linearVelocity);
        }
    }

    public void SetFlying(bool val)
    {
        isFlying = val;
        _trailRender.enabled = val;
    }

    void OnCollisionEnter(Collision cols)
    {
        if (cols.gameObject.GetComponent<Target>() == null) {
            SetFlying(false);
            _rb.useGravity = false;
            _rb.maxAngularVelocity = 0;
            _rb.maxLinearVelocity = 0;
            Invoke(nameof(DestroyArrow), 3f);

            // Play audio & particles
            _audioSrc.Play();
            _particleSys.Play();
        }
    }

    private void DestroyArrow()
    {
        Pooler<Arrow>.Instance.Release(this);
    }
}
