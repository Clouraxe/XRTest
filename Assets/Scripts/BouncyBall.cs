using UnityEngine;
public class BouncyBall : Item
{
    [SerializeField] private int MAX_BOUNCES = 6;
    [SerializeField] private int DISAPPEAR_TIME = 3;
    [SerializeField] private float _bounceScaleMultiplier = 1;
    [SerializeField] private Renderer _render;
    [SerializeField] private ParticleSystem _particSys;
    private int bounces;
    private BallState state = BallState.Idle;


    // Start is called before the first frame update
    protected override void OnStart()
    {
        base.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BallState.Despawning) {
            Color ballColor = _render.material.color;
            _render.material.color = new Color(ballColor.r, ballColor.g, ballColor.b, ballColor.a - (Time.deltaTime / DISAPPEAR_TIME));

            if (ballColor.a <= 0) DestroyObject();
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        bounces++;
        Vector3 contactPos = collision.contacts[0].point;
        _particSys.transform.LookAt(contactPos);
        _particSys.Play();
       if (bounces == MAX_BOUNCES) state = BallState.Despawning;
       else if (bounces < MAX_BOUNCES) transform.localScale *= _bounceScaleMultiplier;
       GetComponent<AudioSource>().Play();
    }
    
    private enum BallState {
        Idle,
        Despawning
    }
}
