using UnityEngine;

public class Wall : MonoBehaviour
{
    public float speed = 1f;

    private Rigidbody body;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        Debug.Log(gameObject.transform.localScale);
        
    }

    void FixedUpdate()
    {
        body.velocity = Vector3.back * speed;
        body.position += body.velocity*Time.fixedDeltaTime;
    }
}
