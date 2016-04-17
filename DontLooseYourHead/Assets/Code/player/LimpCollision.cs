using UnityEngine;

public class LimpCollision : MonoBehaviour
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
        if (transform.parent.name == destroyedPlayer.name) return;

        if (isLast || isMiddle)
        {
            lastHandle.SetActive(false);
            if (middleHandle)
                middleHandle.SetActive(false);
            firstHandle.SetActive(true);
        }
        else if (isFirst)
        {
            lastHandle.SetActive(false);
            if(middleHandle)
                middleHandle.SetActive(false);
            firstHandle.SetActive(false);
        }

        RemoveJoints();
        MoveToDestroyed();
    }

    private void RemoveJoints()
    {
        var joint = GetComponent<CharacterJoint>();
        if(joint)
            joint.breakForce = 1f;
    }

    private void MoveToDestroyed()
    {
        transform.parent = (destroyedPlayer != null) ? destroyedPlayer.transform : null;
    }
}