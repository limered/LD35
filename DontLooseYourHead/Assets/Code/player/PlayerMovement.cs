using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject grabbedHandle = null;
    private Vector3 startPos = Vector3.zero;
    private float movementplaneDistance = 0;

    private LineRenderer lineComponent;

    private void Start()
    {
        lineComponent = GetComponent<LineRenderer>();
        Input.simulateMouseWithTouches = true;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && grabbedHandle)
        {
            MoveGrabbedHandle();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            StartGrabbing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrabbing();
        }
    }

    private void StartGrabbing()
    {
        var rayPoint = GetRayCastPoint(transform.position.z - Camera.main.transform.position.z);

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

                ShowLine();
            }
        }
    }

    private void StopGrabbing()
    {
        grabbedHandle = null;

        HideLine();
    }

    private void MoveGrabbedHandle()
    {
        var rayPoint = GetRayCastPoint(movementplaneDistance);
        var newPosition = new Vector3(rayPoint.x, rayPoint.y, startPos.z);
        grabbedHandle.transform.position = newPosition;

        DrawLine();
    }

    #region LINE

    private void ShowLine()
    {
        lineComponent.enabled = true;

        lineComponent.SetPositions(new Vector3[2] { grabbedHandle.transform.position, GetAnchorPointOfHandle(grabbedHandle) });
    }

    private void HideLine()
    {
        lineComponent.enabled = false;
    }

    private void DrawLine()
    {
        lineComponent.SetPosition(0, grabbedHandle.transform.position);
        lineComponent.SetPosition(1, GetAnchorPointOfHandle(grabbedHandle));
    }

    private Vector3 GetAnchorPointOfHandle(GameObject handle)
    {
        var jointComponent = handle.GetComponent<SpringJoint>();
        return jointComponent.connectedBody.transform.position;
    }

    #endregion LINE

    private Vector3 GetRayCastPoint(float dist)
    {
        var pos = Input.mousePosition;
        //if (Input.touches.Length > 0)
        //    pos = Input.touches[0].position;
        return Camera.main.ScreenPointToRay(pos).GetPoint(dist);
    }

    private float GetDistanceSq(GameObject handle, Vector3 rayPoint)
    {
        var diff = handle.transform.position - rayPoint;
        return new Vector2(diff.x, diff.y).SqrMagnitude();
    }

    public void Die()
    {
        DestroyHandles();
        DestoyCharJoints();
        AddRigidBodies();
    }

    private void DestroyHandles() { }

    private void DestoyCharJoints() { }

    private void AddRigidBodies() { }
}