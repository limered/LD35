using UnityEngine;

public class DestroyComponent : BaseCollider
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
        if (col.gameObject.tag == "Wall" && col.gameObject.tag != "Blood")
        {
            
            transform.parent = (destroyedParent != null) ? destroyedParent.transform : null;

            RemoveAllCharJoints();

            DeleteAllConnectedHandles();

            var rigid = GetComponent<Rigidbody>();
            if (rigid)
            {
                rigid.useGravity = true;
                rigid.mass = 20;
            }

            ActivateBloodEmitter();
            AddFloatRate();
        }
    }

    private void DeleteAllConnectedHandles()
    {
        if (handle != null)
        {
            Destroy(handle);
        }
    }

    private void RemoveAllCharJoints()
    {
        if (transform.childCount > 0) {
            for (var i = transform.childCount-1; i >= 0; i--) {
                var child = transform.GetChild(i);
                child.transform.parent = (destroyedParent != null) ? destroyedParent.transform : null;

                if (child.tag != "Blood")
                {
                    var body = child.gameObject.AddComponent<Rigidbody>();
                    body.useGravity = false;
                }

                var charJoint = child.GetComponent("CharacterJoint");
                if (charJoint != null)
                    (charJoint as CharacterJoint).breakForce = breakForce;
            }
        }
        else
        {
            var charJoint = GetComponent("CharacterJoint");
            if (charJoint != null)
                (charJoint as CharacterJoint).breakForce = breakForce;
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