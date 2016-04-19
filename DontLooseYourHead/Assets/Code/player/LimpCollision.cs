using UnityEngine;

public class LimpCollision : BaseCollider
{
    public bool isLast;
    public bool isMiddle;
    public bool isFirst;

    public GameObject lastHandle;
    public GameObject middleHandle;
    public GameObject firstHandle;

    private GameObject destroyedPlayer;

    // Use this for initialization
    private void Start()
    {
        destroyedPlayer = GameObject.Find("DestroyedPlayer");
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Wall") return;
        if (gameObject.transform.parent == null || gameObject.transform.parent.tag == "Finish") return;

        if (isLast || isMiddle)
        {
            lastHandle.SetActive(false);
            if (middleHandle)
                middleHandle.SetActive(false);
            firstHandle.SetActive(true);

            RemoveJoints();
            MoveToDestroyed();
        }
        else if (isFirst)
        {
            lastHandle.SetActive(false);
            if (middleHandle)
                middleHandle.SetActive(false);
            firstHandle.SetActive(false);

            RemoveJointsSibling();
            MoveToDestroyedSibling();
        }

        ActivateBloodEmitter();
        AddFloatRate();
    }

    private void RemoveJoints()
    {
        var joint = GetComponent<CharacterJoint>();
        if (joint)
            joint.breakForce = 1f;
    }

    private void RemoveJointsSibling()
    {
        var siblings = transform.parent.GetComponentsInChildren<Transform>();
        foreach (var sibling in siblings)
        {
            var joint = sibling.gameObject.GetComponent<CharacterJoint>();
            if (joint)
                joint.breakForce = 1f;
        }
    }

    private void MoveToDestroyed()
    {
        if (tag != "Blood")
        {
            transform.parent = (destroyedPlayer != null) ? destroyedPlayer.transform : null;
            var rigid = GetComponent<Rigidbody>();
            if (rigid)
                rigid.useGravity = true;
        }
    }

    private void MoveToDestroyedSibling()
    {
        var siblings = transform.parent.GetComponentsInChildren<Transform>();
        foreach (var sibling in siblings)
        {
            if (sibling.gameObject.tag != "Blood")
            {
                sibling.parent = (destroyedPlayer != null) ? destroyedPlayer.transform : null;
                var rigid = sibling.gameObject.GetComponent<Rigidbody>();
                if (rigid)
                    rigid.useGravity = true;
            }
        }
    }
}