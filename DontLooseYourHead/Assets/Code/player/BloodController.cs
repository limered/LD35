using UnityEngine;

public class BloodController : MonoBehaviour
{
    public GameObject[] bloodArray;

    private void Start()
    {
        foreach (var go in bloodArray)
        {
            go.SetActive(false);
        }
    }

    public void ActivateBlood(string name)
    {
        foreach (var go in bloodArray)
        {
            if (go.name == name)
                go.SetActive(true);
        }
    }

    public void ActivateAll()
    {
        foreach (var go in bloodArray)
        {
            go.SetActive(true);
        }
    }
}