using UnityEngine;

public class DestroyComponent : MonoBehaviour
{
    public float breakForce = 1f;
    public GameObject handle;

    private GameObject destroyedParent;

    // Use this for initialization
    private void Start()
    {
        destroyedParent = GameObject.Find("DestroyedPlayer");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            transform.parent = (destroyedParent != null) ? destroyedParent.transform : null;

            var rigid = GetComponent<Rigidbody>();
            rigid.useGravity = true;
            rigid.mass = 20;


            var charJoint = GetComponent("CharacterJoint");
            if (charJoint != null)
                (charJoint as CharacterJoint).breakForce = breakForce;
            DeleteAllConnectedHandles();
        }
    }

    private void DeleteAllConnectedHandles()
    {
        if (handle != null)
        {
            Destroy(handle);
        }
    }

    private bool HasComponent(GameObject obj, string ClassType)
    {
        Component[] cs = (Component[])obj.GetComponents(typeof(Component));
        foreach (Component c in cs)
        {
            if (c.GetType().Name == ClassType)
            {
                return true;
            }
        }
        return false;
    }
}