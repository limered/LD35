using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public Player()
    {
        IoC.RegisterSingleton<Player>(this);
    }


    void Start()
    {

    }


    void Update()
    {

    }
}
