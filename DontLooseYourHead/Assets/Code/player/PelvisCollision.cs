using UnityEngine;

public class PelvisCollision : BaseCollider
{
    private GameObject player;
    private GameObject destroyedPlayer;

    public GameObject[] handlesToDelete;
    public GameObject[] legs;

    // Use this for initialization
    private void Start()
    {
        player = GameObject.Find("player");
        destroyedPlayer = GameObject.Find("DestroyedPlayer");
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Wall") return;

        RemoveHandles();
        RemoveJoints();
        AddBody();
        MoveToDestroyed();

        ActivateBloodEmitter();
        AddFloatRate();
    }

    private void RemoveHandles()
    {
        foreach (var handle in handlesToDelete)
        {
            handle.SetActive(false);
        }
    }

    private void RemoveJoints()
    {
        var myJoint = transform.parent.GetComponent<CharacterJoint>();
        if (myJoint != null)
            myJoint.breakForce = 1;

        foreach (var leg in legs)
        {
            var legChildren = leg.GetComponentsInChildren<Transform>();
            foreach (var child in legChildren)
            {
                var joint = child.gameObject.GetComponent<CharacterJoint>();
                if (joint != null)
                    joint.breakForce = 1;
            }
        }
    }

    private void MoveToDestroyed()
    {
        foreach (var leg in legs)
        {
            var legChildren = leg.GetComponentsInChildren<Transform>();
            foreach (var child in legChildren)
            {
                if (child.tag != "Blood")
                    child.parent = (destroyedPlayer != null) ? destroyedPlayer.transform : null;
            }
        }

        var myChildren = GetComponentsInChildren<Transform>();
        foreach (var child in myChildren)
        {
            if (child.tag != "Blood")
                child.parent = (destroyedPlayer != null) ? destroyedPlayer.transform : null;
        }
    }

    private void AddBody()
    {
        foreach (var leg in legs)
        {
            var legChildren = leg.GetComponentsInChildren<Transform>();
            foreach (var child in legChildren)
            {
                if (child.tag != "Blood")
                    child.gameObject.AddComponent<Rigidbody>();
            }
        }

        var myChildren = GetComponentsInChildren<Transform>();
        foreach (var child in myChildren)
        {
            if (child.tag != "Blood")
                child.gameObject.AddComponent<Rigidbody>();
        }
    }
}