using UnityEngine;

public class ShapeCreator
{
    private readonly Game game;
    private readonly System.Random rng = new System.Random();

    public ShapeCreator()
    {
        game = IoC.Resolve<Game>();
    }


    public Shape GenerateNextShape()
    {
        var texture = game.ValidShapes[rng.Next(0, game.ValidShapes.Length)];

        var blocks = new bool[texture.width, texture.height];
        for (int w=0; w<texture.width; w++)
        {
            for (int h = 0; h < texture.height; h++)
            {
                blocks[w, h] = texture.GetPixel(w,h) != Color.black;
            }
        }
        
        return new Shape(blocks, texture.width, texture.height);
    }
}
