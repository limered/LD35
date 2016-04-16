using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject wallPrefab;
    public Vector3 direction = Vector3.back;
    public int wallsAtTheSameTime = 5;
    

    [SerializeField]
    private float spaceBetweenWalls = 100f;

    [SerializeField]
    private float wallSpeed = 5f;

    [SerializeField]
    private List<Wall> walls = new List<Wall>();


    [SerializeField]
    private List<Material> materials = new List<Material>();


    private Material lastMaterial;
    private int wallNr = 0;
    private readonly System.Random rng = new System.Random();

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

        walls.Add(CreateNextWall(spaceBetweenWalls));
    }

    public Wall CreateNextWall(float deltaDistanceToSpawner)
    {
        var wallMat = GetRandomMaterial();

        var nextShape = IoC.Resolve<ShapeCreator>().GenerateNextShape();

        var scale = blockPrefab.transform.localScale;
        var offset = gameObject.transform.position / 2f + new Vector3(scale.x * nextShape.Width / 2f, scale.y * nextShape.Height / 2f);


        //create Wall
        wallNr++;
        var wall = (GameObject)Instantiate(
                        wallPrefab,
                        new Vector3(-nextShape.Width / 2f, -nextShape.Height / 2f, deltaDistanceToSpawner),
                        Quaternion.identity);
        wall.transform.parent = gameObject.transform;
        wall.transform.localScale = new Vector3(nextShape.Width, nextShape.Height, scale.y);
        wall.transform.localRotation = Quaternion.identity;
        wall.transform.localPosition = new Vector3(0, 1, deltaDistanceToSpawner);
        wall.name = "wall #" + wallNr;

        var blocks = new GameObject("blocks");
        blocks.transform.parent = wall.transform;
        blocks.transform.localScale = Vector3.one;
        blocks.transform.localRotation = Quaternion.identity;
        blocks.transform.localPosition = new Vector3((nextShape.Width - scale.x) / 2f + (scale.x / 2f) / nextShape.Width, (nextShape.Height - scale.y) / 2f + (scale.y / 2f) / nextShape.Height);

        //generate blocks
        for (int w = 0; w < nextShape.Width; w++)
        {
            for (int h = 0; h < nextShape.Height; h++)
            {
                if (!nextShape.Blocks[w, h])
                {
                    var block = (GameObject)Instantiate(
                        blockPrefab,
                        new Vector3(w * scale.x + scale.x / 2f - ((nextShape.Width / 2f)), h * scale.y + scale.y / 2f - ((nextShape.Height / 2f))) - offset,
                        Quaternion.identity);
                    block.transform.parent = blocks.transform;
                    block.transform.localPosition = new Vector3((float)w / nextShape.Width, (float)h / nextShape.Height) - new Vector3(offset.x, offset.y);
                    block.transform.localRotation = Quaternion.identity;
                    block.transform.localScale = new Vector3(scale.x / nextShape.Width, scale.y / nextShape.Height, scale.z);
                    block.name = "block (" + w + ", " + h + ")";
                }
            }
        }


        foreach (var ren in wall.GetComponentsInChildren<MeshRenderer>())
        {
            ren.material = wallMat;
        }

        var wallScript = wall.GetComponent<Wall>();
        wallScript.speed = wallSpeed;
        wall.AddComponent<Grow>().timeInSecs = 1f;
        wall.AddComponent<Grow>().timeOffset = 0f;
        return wallScript;
    }

    void Update()
    {
        if (NeedToPlaceNextWall)
        {
            walls.Add(CreateNextWall(
                walls.Any() 
                ? walls.Last().transform.localPosition.z + spaceBetweenWalls
                : spaceBetweenWalls
                ));
        }

        while (walls.Any() 
            && Mathf.Abs(walls.First().transform.localPosition.z - gameObject.transform.localPosition.z) > spaceBetweenWalls*2f)
        {
            var w = walls.First();
            walls.RemoveAt(0);
            Destroy(w.gameObject);
        }
    }

    private bool NeedToPlaceNextWall
    {
        get
        {
            var wallsBeforeMe = walls.Count(x=>x.transform.localPosition.z > gameObject.transform.localPosition.z);
            var lastWall = walls.LastOrDefault();
            return
                wallsBeforeMe < wallsAtTheSameTime;
                //|| Mathf.Abs(lastWall.transform.localPosition.z-gameObject.transform.localPosition.z) < spaceBetweenWalls;
        }
    }

    private Material GetRandomMaterial()
    {
        var mat = lastMaterial;
        if (materials.All(x => x == lastMaterial)) return lastMaterial = materials.FirstOrDefault();

        while (mat == lastMaterial)
        {
            mat = materials[rng.Next(0, materials.Count)];
        }

        return lastMaterial = mat;
    }
}
