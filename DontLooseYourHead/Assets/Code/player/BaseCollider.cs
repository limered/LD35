using UnityEngine;

public class BaseCollider : MonoBehaviour
{
    public GameObject bloodEmitterToActivate;

    protected void ActivateBloodEmitter()
    {
        if (bloodEmitterToActivate)
            bloodEmitterToActivate.SetActive(true);
    }
}