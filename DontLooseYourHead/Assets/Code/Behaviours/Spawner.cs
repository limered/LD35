using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private int wallNr = 0;

    public GameObject blockPrefab;
    public GameObject wallPrefab;
    public Spawner()
    {
        IoC.RegisterSingleton(this);
    }

    void Start()
    {
        if (blockPrefab == null)
        {
            throw new Exception("no block prefab defined");
        }

        var nextShape = IoC.Resolve<ShapeCreator>().GenerateNextShape();

        var scale = blockPrefab.transform.localScale;
        var offset = gameObject.transform.position / 2f + new Vector3(scale.x * nextShape.Width / 2f, scale.y * nextShape.Height / 2f);


        //create Wall
        wallNr++;
        var wall = (GameObject)Instantiate(
                        wallPrefab,
                        new Vector3(-nextShape.Width/2f, -nextShape.Height/2f),
                        Quaternion.identity);
        wall.transform.parent = gameObject.transform;
        wall.transform.localScale = new Vector3(nextShape.Width, nextShape.Height, scale.y);
        wall.transform.localRotation = Quaternion.identity;

        var blocks = new GameObject("blocks");
        blocks.transform.parent = wall.transform;
        blocks.transform.localScale = Vector3.one;
        blocks.transform.localRotation = Quaternion.identity;
        blocks.transform.localPosition = new Vector3((nextShape.Width-scale.x)/2f + (scale.x/2f)/nextShape.Width, (nextShape.Height - scale.y) / 2f + (scale.y / 2f) / nextShape.Height);

        //generate blocks
        for (int w = 0; w < nextShape.Width; w++)
        {
            for (int h = 0; h < nextShape.Height; h++)
            {
                if (!nextShape.Blocks[w, h])
                {
                    var block = (GameObject)Instantiate(
                        blockPrefab,
                        new Vector3(w * scale.x + scale.x / 2f - ((nextShape.Width/2f)), h * scale.y + scale.y/2f - ((nextShape.Height/2f) )) - offset,
                        Quaternion.identity);
                    block.transform.parent = blocks.transform;
                    block.transform.localPosition = new Vector3((float)w/nextShape.Width, (float)h /nextShape.Height) - offset;
                    block.transform.localRotation = Quaternion.identity;
                    block.transform.localScale = new Vector3(scale.x / nextShape.Width, scale.y / nextShape.Height, scale.z);
                    block.name = "block (" + w + ", " + h + ")";
                }
            }
        }
    }

    void Update()
    {

    }
}
