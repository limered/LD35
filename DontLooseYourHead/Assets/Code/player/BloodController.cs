using UnityEngine;

public class BloodController : MonoBehaviour
{
    public GameObject[] BloodArray;

    private void Start()
    {
        foreach (var go in BloodArray)
        {
            go.SetActive(false);
        }
    }

    public void ActivateBlood(string bloodInstanceName)
    {
        foreach (var go in BloodArray)
        {
            if (go.name == bloodInstanceName)
                go.SetActive(true);
        }
    }

    public void ActivateAll()
    {
        foreach (var go in BloodArray)
        {
            go.SetActive(true);
        }
    }
}