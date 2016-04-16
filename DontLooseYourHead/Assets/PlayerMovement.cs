using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {


        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var handles = GameObject.FindGameObjectsWithTag("Line");
            if (handles != null && handles.Length > 0)
            {
                var nearest = handles[0];
                foreach (var handle in handles)
                {
                    
                }
            }
        }

    }
}
