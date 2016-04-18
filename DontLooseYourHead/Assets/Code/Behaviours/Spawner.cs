using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Game game;
    public GameObject blockPrefab;
    public GameObject wallPrefab;
    public Vector3 direction = Vector3.back;

    [SerializeField]
    private float wallSpeed = 5f;

    [SerializeField]
    private Wall currentWall;

    [SerializeField]
    private MeshRenderer backgroundPlane;

    [SerializeField]
    private List<Material> materials = new List<Material>();

    private Material lastMaterial;
    private Material nextMaterial;
    private int wallNr = 0;
    private readonly System.Random rng = new System.Random();

    public Spawner()
    {
        IoC.RegisterSingleton(this);
    }

    void Start()
    {
        game = IoC.Resolve<Game>();

        if (blockPrefab == null)
        {
            throw new Exception("no block prefab defined");
        }

        nextMaterial = GetRandomMaterial();
        CreateNextWall(game.StartShape).StartMoving();
    }

    public Wall CreateNextWall(Texture2D startShape=null)
    {
        var wallMat = nextMaterial;

        var nextShape = IoC.Resolve<ShapeCreator>().GenerateShape(startShape);

        var scale = blockPrefab.transform.localScale;
        var offset = gameObject.transform.position / 2f + new Vector3(scale.x * nextShape.Width / 2f, scale.y * nextShape.Height / 2f);


        //create Wall
        wallNr++;
        var wall = (GameObject)Instantiate(
                        wallPrefab,
                        new Vector3(-nextShape.Width / 2f, -nextShape.Height / 2f, 0f),
                        Quaternion.identity);
        wall.transform.parent = gameObject.transform;
        wall.transform.localScale = new Vector3(nextShape.Width, nextShape.Height, scale.y);
        wall.transform.localRotation = Quaternion.identity;
        wall.transform.localPosition = new Vector3(0, 1, 0f);
        wall.name = "wall #" + wallNr;

        //generate blocks
        var blocks = new GameObject("blocks");
        blocks.transform.parent = wall.transform;
        blocks.transform.localScale = Vector3.one;
        blocks.transform.localRotation = Quaternion.identity;
        blocks.transform.localPosition = new Vector3((nextShape.Width - scale.x) / 2f + (scale.x / 2f) / nextShape.Width, (nextShape.Height - scale.y) / 2f + (scale.y / 2f) / nextShape.Height);

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

        int renderQIndex = 0;
        foreach (var ren in wall.GetComponentsInChildren<MeshRenderer>())
        {
            ren.material = wallMat;
            ren.material.mainTextureOffset = new Vector2((float)rng.NextDouble(), (float)rng.NextDouble());
            ren.material.renderQueue = renderQIndex++;
        }

        currentWall = wall.GetComponent<Wall>();
        currentWall.speed = wallSpeed;
        //wall.AddComponent<Grow>().timeInSecs = 1f;


        lastMaterial = nextMaterial;
        nextMaterial = GetRandomMaterial();
        backgroundPlane.material = nextMaterial;

        //CombineMeshes(currentWall.gameObject);

        return currentWall;
    }

    private void CombineMeshes(GameObject wall)
    {
        MeshFilter[] meshFilters = wall.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        if (wall.transform.GetComponent<MeshFilter>() == null) wall.AddComponent<MeshFilter>();

        
        wall.GetComponent<MeshFilter>().mesh = new Mesh();
        wall.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);
        wall.GetComponent<MeshCollider>().sharedMesh = wall.GetComponent<MeshFilter>().mesh;
        wall.gameObject.SetActive(true);
        wall.transform.DetachChildren();
    }

    void Update()
    {
        if (currentWall == null || !currentWall.isActiveAndEnabled)
        {
            CreateNextWall().StartMoving();
        }

        if (currentWall != null && currentWall.transform.position.z < Camera.main.transform.position.z)
        {
            Destroy(currentWall.gameObject);
            currentWall = null;
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

        return mat;
    }
}
