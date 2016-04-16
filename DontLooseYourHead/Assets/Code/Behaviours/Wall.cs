using UnityEngine;

public class Wall : MonoBehaviour
{
    public float speed = 100f;

    private Rigidbody body;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        Debug.Log(gameObject.transform.localScale);
        
    }

    void FixedUpdate()
    {
        body.AddForce(Vector3.back * speed * body.mass);
        //body.velocity = Vector3.back * speed;
        //body.position += body.velocity*Time.fixedDeltaTime;
    }
}
