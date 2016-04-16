using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject grabbedHandle = null;
    private Vector3 startPos = Vector3.zero;
    private float movementplaneDistance = 0;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0) && grabbedHandle)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var rayPoint = ray.GetPoint(movementplaneDistance);
            var newPosition = new Vector3(rayPoint.x, rayPoint.y, startPos.z);
            grabbedHandle.transform.position = newPosition;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var rayPoint = ray.GetPoint(transform.position.z - Camera.main.transform.position.z);

            var handles = GameObject.FindGameObjectsWithTag("Handle");
            if (handles != null && handles.Length > 0)
            {
                var nearest = handles[0];
                var minDist = GetDistanceSq(nearest, rayPoint);
                foreach (var handle in handles)
                {
                    var newDistance = GetDistanceSq(handle, rayPoint);
                    if (newDistance < minDist)
                    {
                        minDist = newDistance;
                        nearest = handle;
                    }
                }

                if (nearest)
                {
                    grabbedHandle = nearest;
                    movementplaneDistance = nearest.transform.position.z - Camera.main.transform.position.z;
                    startPos = nearest.transform.position;
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            grabbedHandle = null;
        }
    }

    private float GetDistanceSq(GameObject handle, Vector3 rayPoint)
    {
        var diff = handle.transform.position - rayPoint;
        return new Vector2(diff.x, diff.y).SqrMagnitude();
    }
}