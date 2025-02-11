using UnityEngine;
public class BouncyBall : Item
{
    [SerializeField] private int _maxBounces = 6;
    [SerializeField] private int _disappearTime = 3;
    [SerializeField] private float _bounceScaleMultiplier = 1;
    [SerializeField] private Renderer _render;
    [SerializeField] private ParticleSystem _particSys;

    //Components
    private AudioSource _audioSrc;

    private int bounces;
    private BallState state = BallState.Idle;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        _audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BallState.Despawning) {
            Color ballColor = _render.material.color;
            _render.material.color = new Color(ballColor.r, ballColor.g, ballColor.b, ballColor.a - (Time.deltaTime / _disappearTime));

            if (ballColor.a <= 0) DestroyObject();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        bounces++;
        Vector3 contactPos = collision.contacts[0].point;
        _particSys.transform.LookAt(contactPos);
        _particSys.Play();
        if (bounces == _maxBounces) state = BallState.Despawning;
        else if (bounces < _maxBounces) transform.localScale *= _bounceScaleMultiplier;
        _audioSrc.Play();
    }

    private enum BallState
    {
        Idle,
        Despawning
    }
}
