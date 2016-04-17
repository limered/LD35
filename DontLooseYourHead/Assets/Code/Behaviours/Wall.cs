using UnityEngine;

public class Wall : MonoBehaviour
{
    public float speed = 100f;

    private Rigidbody body;

    [SerializeField]
    private float slowMotionFactor = 20f;
    [SerializeField]
    private float lerpInTime = 0.15f;
    private float timeLeft = 0f;

    private Player player;
    [SerializeField]
    private float startSlowmotionAt = 30f;
    [SerializeField]
    private float endSlowmotionAt = 5f;

    [SerializeField] private WallState wallState = WallState.None;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        player = IoC.Resolve<Player>();
    }


    public void StartMoving()
    {
        wallState = WallState.BeforeSlowmotion;
    }

    void Update()
    {
        if (wallState != WallState.None)
        {
            var distanceToPlayer = gameObject.transform.position.z - player.transform.position.z;
            if (distanceToPlayer > 0 && distanceToPlayer <= startSlowmotionAt && distanceToPlayer >= endSlowmotionAt)
            {
                if (wallState == WallState.BeforeSlowmotion)
                {
                    timeLeft = 0f;
                    wallState = WallState.InSlowmotion;
                }

                timeLeft = Mathf.Clamp(timeLeft + Time.deltaTime, 0f, lerpInTime);

                //body.AddForce(Vector3.back * speed * body.mass / slowMotionFactor * Time.deltaTime);
                body.velocity = Vector3.back * speed / Mathf.Clamp(slowMotionFactor * timeLeft / lerpInTime, 1f, slowMotionFactor);
                body.position += body.velocity * Time.deltaTime;
            }
            else
            {
                if (wallState == WallState.InSlowmotion)
                {
                    wallState = WallState.AfterSlowmotion;
                    timeLeft = lerpInTime;
                    body.isKinematic = false;
                }

                timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0f, lerpInTime);

                

                if (wallState == WallState.AfterSlowmotion)
                {

                    body.AddForce(Vector3.back*speed*body.mass*50f);
                }
                else
                {
                    body.velocity = Vector3.back * speed / Mathf.Clamp(slowMotionFactor * timeLeft / lerpInTime, 1f, slowMotionFactor);
                    body.position += body.velocity * Time.deltaTime;
                }
            }
        }
    }

    public enum WallState
    {
        None,
        BeforeSlowmotion,
        InSlowmotion,
        AfterSlowmotion
    }
}
