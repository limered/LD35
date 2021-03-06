﻿using System.Collections.Generic;
using UnityEngine;

public class ShapeCreator
{
    private readonly Dictionary<string, Shape> cache = new Dictionary<string, Shape>();
    private readonly Game game;
    private readonly System.Random rng = new System.Random();
    private Shape? lastShape;

    public ShapeCreator()
    {
        game = IoC.Resolve<Game>();
    }


    public Shape GenerateShape(Texture2D texture=null)
    {
        //get random texture from valid shapes pool
        string name;
        if (texture == null)
        {
            do
            {
                texture = game.ValidShapes[rng.Next(0, game.ValidShapes.Length)];
                name = texture.name;
            } while (lastShape.HasValue && lastShape.Value.Name == name);
        }
        else
        {
            name = texture.name;
        }

        var pixels = texture.GetPixels();
        var width = texture.width;
        var height = texture.height;

        //read shape data
        Shape shape;
        if (!cache.TryGetValue(name, out shape))
        {
            var blocks = new bool[width, height];
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    blocks[w, h] = pixels[h * width + w] != Color.black;
                }
            }

            shape = new Shape(name, blocks, width, height);
            cache.Add(name, shape);
        }

        lastShape = shape;
        return shape;
    }
}
