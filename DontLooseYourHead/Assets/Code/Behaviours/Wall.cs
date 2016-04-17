using UnityEngine;

public class Wall : MonoBehaviour
{
    public float speed = 100f;

    private Rigidbody body;
    private bool isMoving = false;
    [SerializeField]
    private bool isInSlowmotion = false;

    private bool wasInSlowmotion = false;

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

    private int playerLayer;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        player = IoC.Resolve<Player>();

        playerLayer = LayerMask.NameToLayer("Player");
    }


    public void StartMoving()
    {
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            var distanceToPlayer = gameObject.transform.position.z - player.transform.position.z;
            if (distanceToPlayer > 0 && distanceToPlayer <= startSlowmotionAt && distanceToPlayer >= endSlowmotionAt)
            {
                if (!isInSlowmotion)
                {
                    timeLeft = 0f;
                    wasInSlowmotion = true;
                }

                timeLeft = Mathf.Clamp(timeLeft + Time.deltaTime, 0f, lerpInTime);

                isInSlowmotion = true;
                //body.AddForce(Vector3.back * speed * body.mass / slowMotionFactor * Time.deltaTime);
                body.velocity = Vector3.back * speed / Mathf.Clamp(slowMotionFactor * timeLeft / lerpInTime, 1f, slowMotionFactor);
                body.position += body.velocity * Time.deltaTime;
            }
            else
            {
                if (isInSlowmotion)
                {
                    timeLeft = lerpInTime;
                    body.isKinematic = false;
                }

                timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0f, lerpInTime);

                isInSlowmotion = false;

                if (wasInSlowmotion)
                {

                    body.AddForce(Vector3.back*speed*body.mass*50f
                        /// Mathf.Clamp(slowMotionFactor*timeLeft/lerpInTime, 1f, slowMotionFactor)/(wasInSlowmotion ? 4f : 1f)
                        );
                }
                else
                {
                    body.velocity = Vector3.back * speed / Mathf.Clamp(slowMotionFactor * timeLeft / lerpInTime, 1f, slowMotionFactor) / (wasInSlowmotion ? 4f : 1f);
                    body.position += body.velocity * Time.deltaTime;
                }
            }
        }
    }
}
