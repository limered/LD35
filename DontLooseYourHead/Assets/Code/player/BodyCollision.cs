using UnityEngine;
using System.Collections;

public class BodyCollision : MonoBehaviour {

    private GameObject player;
    private GameObject destroyedPlayer;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("player");
        destroyedPlayer = GameObject.Find("DestroyedPlayer");
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Wall") return;

        RemoveAllHandles();
        var children = GetComponentsInChildren<Transform>();
        for (var i = 0; i < children.Length; i++) {
            RemoveJoints(children[i].gameObject);
            MoveToDestroyed(children[i].gameObject);
            AddBody(children[i].gameObject);
        }
    }

    void RemoveAllHandles() {
        var handles = GameObject.FindGameObjectsWithTag("Handle");
        foreach (var handle in handles)
        {
            handle.SetActive(false);
        }
    }

    void RemoveJoints(GameObject go) {
        var joint = go.GetComponent<CharacterJoint>();
        if (joint != null)
            joint.breakForce = 1;
    }

    void MoveToDestroyed(GameObject go) {
        go.transform.parent = (destroyedPlayer != null) ? destroyedPlayer.transform : null;
    }

    void AddBody(GameObject go) {
        var body = go.GetComponent<Rigidbody>();
        if (body != null) {
            body = go.AddComponent<Rigidbody>();
            //body.useGravity = false;
        }
    }
}