using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    public float Blood = 5000;
    public float FloatRate = 0;

    public float tempPoints = 0;

    private readonly Dictionary<string, float> bloodDictionary = new Dictionary<string, float>();

    private readonly Dictionary<string, bool> alreadyRemovedDictionary = new Dictionary<string, bool>();

    private Rect boundsRect = new Rect();

    public Player()
    {
        IoC.RegisterSingleton<Player>(this);
    }


    void Start()
    {
        bloodDictionary.Add("Hand", 1);
        bloodDictionary.Add("Arm_Lower", 2);
        bloodDictionary.Add("Arm_Upper", 2);
                        
        bloodDictionary.Add("Foot", 1);
        bloodDictionary.Add("Leg_Lower", 1);
        bloodDictionary.Add("Leg_Upper", 3);
                        
        bloodDictionary.Add("Pelvis", 5);
        bloodDictionary.Add("Head", 100);
    }


    void Update()
    {
        if (Blood - FloatRate*Time.deltaTime * 10 <= 0)
        {
            Blood = 0;
            IoC.Resolve<Game>().RestartGame();
            return;
        }
        Blood -= FloatRate*Time.deltaTime * 10;

        CalcPoints();
    }

    public void AddFlowRate(string partName, int side)
    {
        if (alreadyRemovedDictionary.ContainsKey(GetDictionaryKey(partName, side))) return;

        FloatRate += bloodDictionary[partName];
        alreadyRemovedDictionary.Add(GetDictionaryKey(partName, side), true);

        PlayHitSound();
    }

    private string GetDictionaryKey(string partName, int side)
    {
        return partName + side;
    }

    public void CalcPoints()
    {
        var allChildren = gameObject.GetComponentsInChildren<Transform>();
        boundsRect.position = GameObject.Find("head").transform.position;
        boundsRect.size = Vector2.zero;
        foreach (var child  in allChildren)
        {
            var body = child.gameObject.GetComponent<Rigidbody>();
            if(body && child.gameObject.tag != "Handle")
                CheckPositionAgainstRect(child, ref boundsRect);
        }
        var score = boundsRect.width + boundsRect.height;
        tempPoints = score;

        Debug.DrawLine(new Vector3(boundsRect.xMin, boundsRect.yMin, 0), new Vector3(boundsRect.xMax, boundsRect.yMax, 0));
    }

    private void PlayHitSound()
    {
        var sound = gameObject.GetComponent<AudioSource>();
        var pitch = Random.value*1f - 0.5f;
        sound.pitch = 1f + pitch;
        sound.PlayOneShot(sound.clip);
    }

    private void CheckPositionAgainstRect(Transform trans, ref Rect boundRect)
    {
        var position = trans.position;
        if (position.x < boundRect.xMin)
            boundRect.xMin = position.x;
        if (position.x > boundRect.xMax)
            boundRect.xMax = position.x;
        if (position.y < boundRect.yMin)
            boundRect.yMin = position.y;
        if (position.y > boundRect.yMax)
            boundRect.yMax = position.y;
    }
}
