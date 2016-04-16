using UnityEngine;
using System.Collections;

public class Grow : MonoBehaviour
{
    private Vector3 destinationScale;


    public float timeOffset = 0f;

    public float timeInSecs = 0.5f;

    private float timePassed = 0f;


    public Grow()
    {

    }

    public Grow(float timeInSecs, float timeOffset=0f)
    {
        this.timeInSecs = timeInSecs;
        this.timeOffset = timeOffset;
    }

    void Start()
    {
        destinationScale = gameObject.transform.localScale;

        timePassed = timeOffset;

        gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, destinationScale, Mathf.Clamp(timePassed / timeInSecs, 0f, 1f));
    }


    void Update()
    {
        timePassed += Time.deltaTime;
        gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, destinationScale, Mathf.Clamp(timePassed / timeInSecs, 0f, 1f));
        if (timePassed >= timeInSecs)
        {
            Destroy(this);
        }
    }
}
