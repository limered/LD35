using UnityEngine;
using System.Collections;

public class SoundComponent : MonoBehaviour
{

    private AudioSource audio;

    public SoundComponent()
    {
        IoC.RegisterSingleton(this);
    }

    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        
    }

    public void PlaySlowdown()
    {
        audio.time = 0f;
        audio.pitch = 1f;
        audio.Play();
    }

    public void PlaySpeedup()
    {
        audio.time = audio.clip.length - 0.1f;
        audio.Play();
        audio.pitch = -2.5f;
    }
}
