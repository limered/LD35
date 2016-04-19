using UnityEngine;

public class BaseCollider : MonoBehaviour
{
    public GameObject bloodEmitterToActivate;
    public string partName;
    public int side;

    protected void ActivateBloodEmitter()
    {
        if (bloodEmitterToActivate)
            bloodEmitterToActivate.SetActive(true);
    }

    protected void AddFloatRate()
    {
        if (!string.IsNullOrEmpty(partName))
        {
            IoC.Resolve<Player>().AddFlowRate(partName, side);
        }
    }
}